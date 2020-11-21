#include "BluetoothSerial.h"
#include <ArduinoJson.h>
#include <iostream>
#include <vector>
#include <sstream>
#include <string>
#include <iostream>
#include <stdio.h>
#include <stdlib.h>

//#define F 2        //Focus Common
//#define S1 3       //Shutter1
#define S1 0  //Shutter1
#define S2 4  //Shutter2
#define S3 16 //Shutter3
#define S4 17 //Shutter4
#define S5 18 //Shutter5
#define F 23  //Focus Common

#define defu_time 200
#define time_sleep_task 100
#if !defined(CONFIG_BT_ENABLED) || !defined(CONFIG_BLUEDROID_ENABLED)
#error Bluetooth is not enabled! Please run `make menuconfig` to and enable it
#endif
BluetoothSerial SerialBT;


char String_data; // for incoming serial data
bool camera1_flag = false;
bool camera2_flag = false;
bool camera3_flag = false;
bool camera4_flag = false;
bool camera5_flag = false;
int camera1_time = 200;
int camera2_time = 500;
int camera3_time = 300;
int camera4_time = 200;
int camera5_time = 100;
int camera_time[] = {100, 100, 100, 100, 100};

TaskHandle_t TaskHandle_1;
TaskHandle_t TaskHandle_2;
TaskHandle_t TaskHandle_3;
TaskHandle_t TaskHandle_4;
TaskHandle_t TaskHandle_5;
void setup()
{
  Serial.begin(115200);// opens serial port, sets data rate to 115200 bps
  SerialBT.begin("ESP32_Camera_Control"); //Bluetooth device name
  Serial.println("The device started, now you can pair it with bluetooth!");
  pinMode(LED_BUILTIN, OUTPUT); //Specify that LED pin is output
  pinMode(S1, OUTPUT);
  pinMode(S2, OUTPUT);
  pinMode(S3, OUTPUT);
  pinMode(S4, OUTPUT);
  pinMode(S5, OUTPUT);
  pinMode(F, OUTPUT);
  xTaskCreate(&camera_Task1, "Camera1", 1024, NULL, 10, &TaskHandle_1);
  xTaskCreate(&camera_Task2, "Camera2", 1024, NULL, 10, &TaskHandle_2);
  xTaskCreate(&camera_Task3, "Camera3", 1024, NULL, 10, &TaskHandle_3);
  xTaskCreate(&camera_Task4, "Camera4", 1024, NULL, 10, &TaskHandle_4);
  xTaskCreate(&camera_Task5, "Camera5", 1024, NULL, 10, &TaskHandle_5);
  Serial.println("Ready");
}


void Pin_HiGH()
{
  digitalWrite(S1, HIGH);
  digitalWrite(S2, HIGH);
  digitalWrite(S3, HIGH);
  digitalWrite(S4, HIGH);
  digitalWrite(S5, HIGH);
  digitalWrite(F, HIGH);
}

void Freeze_mode() {
  digitalWrite(F, LOW);
  Serial.println("Focus");
  delay(100);
  digitalWrite(F, HIGH);
  digitalWrite(S1, LOW);
  digitalWrite(S2, LOW);
  digitalWrite(S3, LOW);
  digitalWrite(S4, LOW);
  digitalWrite(S5, LOW);
  delay(200);
  Pin_HiGH();
  Serial.println("Shutters");
}

void sutter_camera(char camera_number, int delay_time) {
  switch (camera_number) {
    case '1':
      Serial.println("Camera1");
      delay(delay_time);
      digitalWrite(S1, LOW);
      break;
    case '2':
      Serial.println("Camera2");
      delay(delay_time);
      digitalWrite(S2, LOW);
      break;
    case '3':
      Serial.println("Camera3");
      delay(delay_time);
      digitalWrite(S3, LOW);
      break;
    case '4':
      Serial.println("Camera4");
      delay(delay_time);
      digitalWrite(S4, LOW);
      break;
    case '5':
      Serial.println("Camera5");
      delay(delay_time);
      digitalWrite(S5, LOW);
      break;
  }
}

void sutter_camera_custom(char camera_number) {
  switch (camera_number) {
    case '1':
      camera1_flag = true;
      break;
    case '2':
      camera2_flag = true;
      break;
    case '3':
      camera3_flag = true;
      break;
    case '4':
      camera4_flag = true;
      break;
    case '5':
      camera5_flag = true;
      break;
  }
}

void camera_Task1(void *p)
{
  while (1)
  {
    if (camera1_flag)
    {
      delay(camera_time[0]);
      digitalWrite(S1, LOW);
      Serial.println("Camera1_Task1");
      delay(defu_time);
      digitalWrite(S1, HIGH);
      vTaskSuspend(TaskHandle_1);
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
      delay(camera_time[1]);
      digitalWrite(S2, LOW);
      Serial.println("Camera2_Task2");
      delay(defu_time);
      digitalWrite(S2, HIGH);
      vTaskSuspend(TaskHandle_2);
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
      delay(camera_time[2]);
      digitalWrite(S3, LOW);
      Serial.println("Camera3_Task3");
      delay(defu_time);
      digitalWrite(S3, HIGH);
      vTaskSuspend(TaskHandle_3);
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
      delay(camera_time[3]);
      digitalWrite(S4, LOW);
      Serial.println("Camera4_Task4");
      delay(defu_time);
      digitalWrite(S4, HIGH);
      vTaskSuspend(TaskHandle_4);
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
      delay(camera_time[4]);
      Serial.println("Camera5_Task5");
      digitalWrite(S5, LOW);
      delay(defu_time);
      digitalWrite(S5, HIGH);
      vTaskSuspend(TaskHandle_5);
    }
    delay(time_sleep_task);
  }
}

void task_resume() {
  vTaskResume(TaskHandle_1);
  vTaskResume(TaskHandle_2);
  vTaskResume(TaskHandle_3);
  vTaskResume(TaskHandle_4);
  vTaskResume(TaskHandle_5);
}

String val_string;
int time_delay;
void loop()
{
  //digitalWrite(LED_BUILTIN, HIGH);
  if (Serial.available() > 0)
  {
    String_data = Serial.read();
    Serial.println(String_data);
    if (String_data == 'T')
    {
      Freeze_mode();
    }
  } else if (SerialBT.available()) {
    val_string = SerialBT.readString();
    Serial.println(val_string);
    DynamicJsonDocument doc(2048);
    deserializeJson(doc, val_string);
    JsonObject obj = doc.as<JsonObject>();
    String camera_list = obj["cameralists"];
    int MODE = obj["mode"];
    String str_delay_time = obj["delay_time"];
    time_delay = str_delay_time.toInt();
    String delay_time_customs = obj["delay_time_customs"];
    switch (MODE) {
      case 0:
        //freeze mode
        Freeze_mode();
        break;
      case 1:
        for (int index = 0; index < camera_list.length(); index++) {
          // Serial.println(camera_list[index]);
          if (camera_list[index] != ',') {
            sutter_camera(camera_list[index], time_delay);
          }
        }
        delay(50);
        Pin_HiGH();
        break;
      case 2:
        //Serial.println(delay_time_customs);
        int substring_next;
        bool frist_time = true;
        int counting = 1;
        digitalWrite(F, LOW);
        for (int i = 0; i < delay_time_customs.length(); i++) {
          if (delay_time_customs.substring(i, i + 1) == ",") {
            if (frist_time) {
              camera_time[0] = delay_time_customs.substring(0, i).toInt();
              Serial.println(camera_time[0]);
              frist_time = false;
            }
            camera_time[counting] = delay_time_customs.substring(i + 1).toInt();
            Serial.println(camera_time[counting]);
            counting++;
          }
        }
        for (int index = 0; index < camera_list.length(); index++) {
          if (camera_list[index] != ',') {
            sutter_camera_custom(camera_list[index]);
          }
        }
        task_resume();
        break;
    }
  }
  //delay(200);
  //digitalWrite(LED_BUILTIN, LOW);
}
