#include <sys/stat.h>
#include <sys/types.h>
#include <fcntl.h>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>

#define IN  0
#define OUT 1

#define LOW  0
#define HIGH 1

#define PIN_LIGHT_1  4
#define PIN_LIGHT_2  17
#define PIN_LIGHT_3  27
#define PIN_LIGHT_4  22
#define PIN_LIGHT_5  23

#define PIN_DOOR_OUT_1 24
#define PIN_DOOR_OUT_2 16
#define PIN_DOOR_OUT_3 20
#define PIN_DOOR_OUT_4 21

#define PIN_DOOR_IN_1  25
#define PIN_DOOR_IN_2  13
#define PIN_DOOR_IN_3  19
#define PIN_DOOR_IN_4  26

int init(int pin)
{
	#define BUFFER_MAX 3
	char buffer[BUFFER_MAX];
	ssize_t bytes_written;
	int fd;
	fd = open("/sys/class/gpio/export", O_WRONLY);
	if (-1 == fd) {
		fprintf(stderr, "Failed to open export for writing!\n");
		return(-1);
	}
	bytes_written = snprintf(buffer, BUFFER_MAX, "%d", pin);
	write(fd, buffer, bytes_written);
	close(fd);
	return(0);
}

int finish(int pin)
{
	char buffer[BUFFER_MAX];
	ssize_t bytes_written;
	int fd;
	fd = open("/sys/class/gpio/unexport", O_WRONLY);
	if (-1 == fd) {
		fprintf(stderr, "Failed to open unexport for writing!\n");
		return(-1);
	}
	bytes_written = snprintf(buffer, BUFFER_MAX, "%d", pin);
	write(fd, buffer, bytes_written);
	close(fd);
	return(0);
}

int pinMode(int pin, int mode)
{
	static const char s_directions_str[]  = "in\0out";
	#define DIRECTION_MAX 35
	char path[DIRECTION_MAX];
	int fd;
	snprintf(path, DIRECTION_MAX, "/sys/class/gpio/gpio%d/direction", pin);
	fd = open(path, O_WRONLY);
	if (-1 == fd) {
		fprintf(stderr, "Failed to open gpio direction for writing!\n");
		return(-1);
	}
	if (-1 == write(fd, &s_directions_str[IN == mode ? 0 : 3], IN == mode ? 2 : 3)) {
		fprintf(stderr, "Failed to set direction!\n");
		return(-1);
	}
	close(fd);
	return(0);
}

int digitalRead(int pin)
{
	#define VALUE_MAX 30
	char path[VALUE_MAX];
	char value_str[3];
	int fd;
	snprintf(path, VALUE_MAX, "/sys/class/gpio/gpio%d/value", pin);
	fd = open(path, O_RDONLY);
	if (-1 == fd) {
		fprintf(stderr, "Failed to open gpio value for reading!\n");
		return(-1);
	}
	if (-1 == read(fd, value_str, 3)) {
		fprintf(stderr, "Failed to read value!\n");
		return(-1);
	}
	close(fd);
	return(atoi(value_str));
}

int digitalWrite(int pin, int value)
{
	static const char s_values_str[] = "01";
	char path[VALUE_MAX];
	int fd;
	snprintf(path, VALUE_MAX, "/sys/class/gpio/gpio%d/value", pin);
	fd = open(path, O_WRONLY);
	if (-1 == fd) {
		fprintf(stderr, "Failed to open gpio value for writing!\n");
		return(-1);
	}
	if (1 != write(fd, &s_values_str[LOW == value ? 0 : 1], 1)) {
		fprintf(stderr, "Failed to write value!\n");
		return(-1);
	}
	close(fd);
	return(0);
}

void blink(int pin, int frec, int time)
{
	if (-1 == init(pin))
		return(1);
	if (-1 == pinMode(pin, OUT))
		return(2);

	int ctdrTime = 0;
	while(ctdrTime == time)
	{
		if (-1 == digitalWrite(pin, ctdrTime % 2))
			return(3);
		printf("Blinking pin #%d", pin);
		usleep(frec * 1000);
		ctdrTime++;
	}
}

void lightOn(int pin)
{
	digitalWrite(pin, HIGH);	
}

void lightOff(int pin)
{
	digitalWrite(pin, LOW);
}

int readDoor(int pin)
{
	digitalRead(pin);
}

void initLights()
{
	if (-1 == init(PIN_LIGHT_1) )
		return(1);
	if (-1 == init(PIN_LIGHT_2) )
		return(2);
	if (-1 == init(PIN_LIGHT_3) )
		return(3);
	if (-1 == init(PIN_LIGHT_4) )
		return(4);
	if (-1 == init(PIN_LIGHT_5) )
		return(4);

	if (-1 == pinMode(PIN_LIGHT_1, OUT) )
		return(5);
	if (-1 == pinMode(PIN_LIGHT_2, OUT) )
		return(5);
	if (-1 == pinMode(PIN_LIGHT_3, OUT) )
		return(5);
	if (-1 == pinMode(PIN_LIGHT_4, OUT) )
		return(5);
	if (-1 == pinMode(PIN_LIGHT_5, OUT) )
		return(5);
}

void initDoors()
{
	if (-1 == init(PIN_DOOR_OUT_1) || -1 == init(PIN_DOOR_IN_1) )
		return(1);
	if (-1 == init(PIN_DOOR_OUT_2) || -1 == init(PIN_DOOR_IN_2) )
		return(2);
	if (-1 == init(PIN_DOOR_OUT_3) || -1 == init(PIN_DOOR_IN_3) )
		return(3);
	if (-1 == init(PIN_DOOR_OUT_4) || -1 == init(PIN_DOOR_IN_4) )
		return(4);


	if (-1 == pinMode(PIN_DOOR_OUT_1, OUT) || -1 == pinMode(PIN_DOOR_IN_1, IN) )
		return(5);
	if (-1 == pinMode(PIN_DOOR_OUT_2, OUT) || -1 == pinMode(PIN_DOOR_IN_2, IN) )
		return(6);
	if (-1 == pinMode(PIN_DOOR_OUT_3, OUT) || -1 == pinMode(PIN_DOOR_IN_3, IN) )
		return(7);
	if (-1 == pinMode(PIN_DOOR_OUT_4, OUT) || -1 == pinMode(PIN_DOOR_IN_4, IN) )
		return(8);

	digitalWrite(PIN_DOOR_OUT_1, HIGH);
	digitalWrite(PIN_DOOR_OUT_2, HIGH);
	digitalWrite(PIN_DOOR_OUT_3, HIGH);
	digitalWrite(PIN_DOOR_OUT_4, HIGH);
}
