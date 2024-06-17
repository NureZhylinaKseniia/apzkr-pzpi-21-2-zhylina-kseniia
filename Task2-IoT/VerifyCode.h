
bool VerifyCode(String serverUrl, int code, int roomId, tm timestamp)
{
  HTTPClient http;
  String url = serverUrl + "api/booking/checkCode/" + String(roomId);

  http.begin(url);
  http.addHeader("Content-Type", "application/json");

  char formattedTime[25];
  strftime(formattedTime, sizeof(formattedTime), "%Y-%m-%dT%H:%M:%S.000Z", &timestamp);
  String notificationDateTime = String(formattedTime);

  DynamicJsonDocument jsonDocument(1024);
  jsonDocument["code"] = code;
  jsonDocument["timestamp"] = formattedTime;

  String jsonPayload;
  serializeJson(jsonDocument, jsonPayload);

  int httpResponseCode = http.POST(jsonPayload);

  Serial.printf("HTTP Response code: %d\n", httpResponseCode);
  if (httpResponseCode >= 200 && httpResponseCode <= 400) 
  {
    DynamicJsonDocument jsonDocumentResponse(1024);
    deserializeJson(jsonDocumentResponse, http.getString());

    if (jsonDocumentResponse["code"].as<int>() == code)
    {
      return true;
    }
  }
  return false;
}