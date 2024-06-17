#include <LiquidCrystal_I2C.h>
#include <Keypad.h>

#define ROW_NUM     4 // four rows
#define COLUMN_NUM  4 // four columns

char keys[ROW_NUM][COLUMN_NUM] = 
{
  {'1','2','3', 'A'},
  {'4','5','6', 'B'},
  {'7','8','9', 'C'},
  {'*','0','#', 'D'}
};

byte pin_rows[ROW_NUM]      = {19,18,5,17};
byte pin_column[COLUMN_NUM] = {16,4,2,15};

LiquidCrystal_I2C lcd(0x27, 16, 2);
Keypad keypad = Keypad(makeKeymap(keys), pin_rows, pin_column, ROW_NUM, COLUMN_NUM);

const int maxInput = 6;

char inputArray[maxInput]; // Array to store entered characters (changed to size maxInput)
int arrayIndex = 0; // Index to keep track of the current position in the array

void addCharacter(char key) 
{
  if (arrayIndex < maxInput) 
  {
    inputArray[arrayIndex++] = key;
  }
}

void deleteLastCharacter() 
{
  if (arrayIndex > 0) {
    arrayIndex--;
    inputArray[arrayIndex] = '\0';
  }
}

void clearArray() 
{
  arrayIndex = 0;
  for (int i = 0; i < maxInput; i++) {
    inputArray[i] = '\0';
  }
}

void printToLCD() 
{
  lcd.clear();
  lcd.setCursor(0, 0);
  lcd.print(inputArray);
}

void printMessageToLCD(String message)
{
  lcd.clear();
  lcd.setCursor(0, 0);
  lcd.print(message);
}

void clearLCD(bool outputMessage) 
{
  lcd.clear();
  if(outputMessage)
  {
    lcd.setCursor(0, 0);
    lcd.print("Input");
    lcd.setCursor(0, 1);
    lcd.print("Cleared");
    delay(2000);
    lcd.clear();
  }
}
