
#include "soar_imu.h"
SOAR_IMU imu_device;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  imu_device.BNO_SETUP();
}

void loop() {
  // put your main code here, to run repeatedly:
  // float *accel = imu_device.GET_ACCELERATION();
  // float *linear = imu_device.GET_LINEARACCEL();
  // float *gravity = imu_device.GET_GRAVITY();
  float *gyro = imu_device.GET_GYROSCOPE();
  float *quat = imu_device.GET_QUAT();
  float *euler = imu_device.GET_EULER();
  //print the gyro values
  Serial.print("Gyro: ");
  Serial.print(gyro[0]);
  Serial.print(", ");
  Serial.print(gyro[1]);
  Serial.print(", ");
  Serial.println(gyro[2]);
  Serial.println("-----------------------------------");


  // delete[] accel;
  // delete[] linear;
  // delete[] gravity;
  delete[] gyro;
  delete[] quat;
  delete[] euler;
  delay(500);
}
