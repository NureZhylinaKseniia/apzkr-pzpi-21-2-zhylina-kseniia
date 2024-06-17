#include <WiFi.h>
#include <HTTPClient.h>
#include <ArduinoJson.h>
#include <DHT.h>
#include <RTClib.h>
#include <ESP32Servo.h>
#include <Keypad.h>
#include <LiquidCrystal_I2C.h>
#include <Wire.h>
#include "InputCode.h"
#include "VerifyCode.h"

#define NTP_SERVER "pool.ntp.org"
#define UTC_OFFSET 3600
#define UTC_OFFSET_DST 0

const String ssid = "Wokwi-GUEST";
const String password = "";
const String serverUrl = "http://0.tcp.eu.ngrok.io:19737/";

int roomId = 1;

const int servoPin = 14;

Servo servo; 

void setup() 
{
  lcd.init();

  servo.attach(servoPin);
  servo.write(90);

  Serial.begin(115200);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.println("Connecting to WiFi...");
  }
  Serial.println("Connected to WiFi");

  configTime(UTC_OFFSET, UTC_OFFSET_DST, NTP_SERVER);
  tm timeInfo;
  getLocalTime(&timeInfo);
}

void OpenDoor()
{
  int initPos = 90;
  servo.write(initPos + 25);
  delay(1000);
  servo.write(initPos);
}

void loop() 
{
  static tm timeInfo;
  getLocalTime(&timeInfo);
  
  char key = keypad.getKey();

  switch (key) 
  {
    case 'C':
    {
      clearArray();
      clearLCD(true);
      break;
    } 
    case 'D':
    {
      deleteLastCharacter();
      printToLCD();
      break;
    } 
    case 'A':
    {
      bool verified = VerifyCode(serverUrl, atoi(inputArray), roomId, timeInfo);
      if(verified)
      {
        printMessageToLCD("Code accepted");
        OpenDoor();
      }
      else
      {
        printMessageToLCD("Code denied");
      }
      delay(2000);
      clearArray();
      clearLCD(false);
      break;
    } 
    case 'B':
    {
      break;
    } 
    case '*':
    {
      break;
    } 
    case '#':
    {
      break;
    } 
    default: 
    {
      if(isdigit(key))
      {
        addCharacter(key);
        printToLCD();
      }
    }
  }
}