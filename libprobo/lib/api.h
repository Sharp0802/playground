#ifndef API_H
#define API_H

#if __cplusplus
extern "C" {
#endif

void start(void);
void end(void);

void delay(unsigned int count);

void on(unsigned char output_port);
void off(unsigned char output_port);

void motor1(signed char velocity);
void motor2(signed char velocity);
void motor3(signed char velocity);
void motor4(signed char velocity);

void wheel(signed char left, signed char right);
void wheel2(signed char left, signed char right);

void motor(signed char left, signed char right, unsigned int mdelay);
void allmotor(signed char m1, signed char m2, signed char m3, signed char m4);

void servo(unsigned char number, unsigned char position);

void play(unsigned int sound, unsigned int note);
void buzzer(unsigned int mel, unsigned char count, unsigned int ontime, unsigned int offtime);
void tempo(unsigned char data);
void melody(unsigned int data);

unsigned char user_random(unsigned char number);

void timer(unsigned int data);
unsigned int timer_read(void);

void start_melody(unsigned char note);
void end_melody(unsigned char note);

void level_up_melody(unsigned char note);
void level_down_melody(unsigned char note);

unsigned char EEPROM_read(unsigned int uiAddress);
void EEPROM_write(unsigned int uiAddress, unsigned char ucData);

void motor_current(unsigned char m1, unsigned char m2);

void sound(unsigned char port, unsigned char addr, unsigned char loop);
unsigned char sound_read(unsigned char port);

extern volatile unsigned char ROBOT_LEVEL;
extern volatile unsigned char KEYCOUNT[4];
extern volatile unsigned char INPUT;
extern volatile unsigned char ADCDATA[6];
extern volatile unsigned char PSDATA[5];
extern volatile unsigned char BOTH_EDGE, FALLING_EDGE, RISING_EDGE;
extern volatile unsigned char _S1234[16];

extern volatile unsigned char __fea[4];
extern volatile unsigned char __rea[4];
extern volatile unsigned char __bea[4];

#ifdef __cplusplus
}
#endif

#endif

