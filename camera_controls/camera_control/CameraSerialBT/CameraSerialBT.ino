#include "BluetoothSerial.h"
#include <ArduinoJson.h>
#include <vector>
#include <string>
#include <sstream>
#include <iostream>
#include <stdio.h>
#include <stdlib.h>

#define S1 0  //Shutter1
#define S2 4  //Shutter2
#define S3 16 //Shutter3
#define S4 17 //Shutter4
#define S5 18 //Shutter5
#define F 23  //Focus Common

#define time_sleep_task 100
#define defu_time 200
#if !defined(CONFIG_BT_ENABLED) || !defined(CONFIG_BLUEDROID_ENABLED)
#error Bluetooth is not enabled! Please run `make menuconfig` to and enable it
#endif
BluetoothSerial SerialBT;

bool camera1_flag = false;
bool camera2_flag = false;
bool camera3_flag = false;
bool camera4_flag = false;
bool camera5_flag = false;
int camera1_time = 200;
int camera2_time = 200;
int camera3_time = 200;
int camera4_time = 200;
int camera5_time = 200;
bool mainFlag = true;

void camera_Task1(void *p)
{
  while (1)
  {
    if (camera1_flag)
    {
      Serial.println("Camera1");
      delay(camera1_time);
    }
    delay(time_sleep_task);
  }
}

void camera_Task2(void *p)
{
  while (1)
  {
    if (camera2_flag)
    {
      Serial.println("Camera2");
      delay(camera2_time);
    }
    delay(time_sleep_task);
  }
}

void camera_Task3(void *p)
{
  while (1)
  {
    if (camera3_flag)
    {
      Serial.println("Camera3");
      delay(camera3_time);
    }
    delay(time_sleep_task);
  }
}

void camera_Task4(void *p)
{
  while (1)
  {
    if (camera4_flag)
    {
      Serial.println("Camera4");
      delay(camera4_time);
    }
    delay(time_sleep_task);
  }
}

void camera_Task5(void *p)
{
  while (1)
  {
    if (camera5_flag)
    {
      Serial.println("Camera5");
      delay(camera5_time);
    }
    delay(time_sleep_task);
  }
}

void init_pin(){
    digitalWrite(F, HIGH);
    digitalWrite(S1, HIGH);
    digitalWrite(S2, HIGH);
    digitalWrite(S3, HIGH);
    digitalWrite(S4, HIGH);
    digitalWrite(S5, HIGH);
}


void setup()
{
  Serial.begin(115200);
  SerialBT.begin("ESP32_Camera_Control"); //Bluetooth device name
  Serial.println("The device started, now you can pair it with bluetooth!");
  pinMode(LED_BUILTIN, OUTPUT); //Specify that LED pin is output
  pinMode(S1, OUTPUT);
  pinMode(S2, OUTPUT);
  pinMode(S3, OUTPUT);
  pinMode(S4, OUTPUT);
  pinMode(S5, OUTPUT);
  pinMode(F, OUTPUT);
  xTaskCreate(&camera_Task1, "Camera1", 1024, NULL, 10, NULL);
  xTaskCreate(&camera_Task2, "Camera2", 1024, NULL, 10, NULL);
  xTaskCreate(&camera_Task3, "Camera3", 1024, NULL, 10, NULL);
  xTaskCreate(&camera_Task4, "Camera4", 1024, NULL, 10, NULL);
  xTaskCreate(&camera_Task5, "Camera5", 1024, NULL, 10, NULL);
  init_pin();
}



bool flag = false;
String val_string;
void loop()
{
  digitalWrite(LED_BUILTIN, HIGH); // turn the LED on (HIGH is the voltage level)
  if (SerialBT.available())
  {
    val_string = SerialBT.readString();
    if (val_string)
    {
      flag = true;
    }
    else
    {
      flag = false;
    }
  }

  if (flag)
  {
    init_pin();
    DynamicJsonDocument doc(1024);
    deserializeJson(doc, val_string);
    JsonObject obj = doc.as<JsonObject>();
    camera1_flag = obj["camera1_flag"];
    camera2_flag = obj["camera2_flag"];
    camera3_flag = obj["camera3_flag"];
    camera4_flag = obj["camera4_flag"];
    camera5_flag = obj["camera5_flag"];
    camera1_time = camera1_time + atoi(obj["camera1_time"]);
    camera2_time = camera2_time + atoi(obj["camera2_time"]);
    camera3_time = camera3_time + atoi(obj["camera3_time"]);
    camera4_time = camera4_time + atoi(obj["camera4_time"]);
    camera5_time = camera5_time + atoi(obj["camera5_time"]);
    Serial.println("Task1");
    flag = false;
  }
  else
  {
    camera1_flag = false;
    camera2_flag = false;
    camera3_flag = false;
    camera4_flag = false;
    camera5_flag = false;
    camera1_time = defu_time;
    camera2_time = defu_time;
    camera3_time = defu_time;
    camera4_time = defu_time;
    camera5_time = defu_time;
  }
  delay(200);
  digitalWrite(LED_BUILTIN, LOW);
}
