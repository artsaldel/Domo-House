
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <sys/socket.h>
#include <arpa/inet.h>
#include <netdb.h>
#include <signal.h>
#include <fcntl.h>

#define CONNMAX 1000
#define BYTES 1024


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



char *ROOT;
int listenfd, clients[CONNMAX];

/*
*************************************************************************************************************************************
**************************************************ALL FORK SERVER********************************************************************
*************************************************************************************************************************************
*/

void startDomoHouse()
{
    //Star connection with rasp's gpios
    initLights();
    initDoors();
    beginConnection();
}

void beginConnection()
{

	FILE *fpp;
	fpp=fopen("/casa.txt", "w");
	fprintf(fpp, "false.false.false.false.false.false.false.false.false.");
	fclose(fpp);

	struct sockaddr_in clientaddr;
    socklen_t addrlen;
    char c;    
    
    //Default Values PATH = ~/ and PORT=10000
    char PORT[6];
    ROOT = getenv("PWD");
    strcpy(PORT,"8033");

    int slot=0;
   
    printf("Server started at port no. %s%s%s with root directory as %s%s%s\n","\033[92m",PORT,"\033[0m","\033[92m",ROOT,"\033[0m");
    // Setting all elements to -1: signifies there is no client connected
    int i;
    for (i = 0; i < CONNMAX; i++)
        clients[i] = -1;
    
    startServer(PORT);

    // ACCEPT connections
    while (1)
    {
        addrlen = sizeof(clientaddr);
        clients[slot] = accept (listenfd, (struct sockaddr *) &clientaddr, &addrlen);

        if (clients[slot]<0)
            error ("accept() error");
        else
        {
            if ( fork() == 0 )
            {
                respond(slot);
                exit(0);
            }
        }

        while (clients[slot]!=-1) slot = (slot+1)%CONNMAX;
    }
}

//start server
void startServer(char *port)
{
    struct addrinfo hints, *res, *p;

    // getaddrinfo for host
    memset (&hints, 0, sizeof(hints));
    hints.ai_family = AF_INET;
    hints.ai_socktype = SOCK_STREAM;
    hints.ai_flags = AI_PASSIVE;

    if (getaddrinfo( NULL, port, &hints, &res) != 0)
    {
        perror ("getaddrinfo() error");
        exit(1);
    }

    // socket and bind
    for (p = res; p != NULL; p=p->ai_next)
    {
        listenfd = socket (p->ai_family, p->ai_socktype, 0);
        if (listenfd == -1) continue;
        if (bind(listenfd, p->ai_addr, p->ai_addrlen) == 0) break;
    }

    if (p==NULL)
    {
        perror ("socket() or bind()");
        exit(1);
    }

    freeaddrinfo(res);

    // listen for incoming connections
    if ( listen (listenfd, 1000000) != 0 )
    {
        perror("listen() error");
        exit(1);
    }
}

//client connection
void respond(int n)
{
    char mesg[99999], *reqline[3], data_to_send[BYTES], path[99999];
    int rcvd, fd, bytes_read;

    memset( (void*)mesg, (int)'\0', 99999 );

    rcvd=recv(clients[n], mesg, 99999, 0);

    printf("%s", mesg);
    reqline[0] = strtok (mesg, " \t\n");
    if ( strncmp(reqline[0], "GET\0", 4) == 0 || strncmp(reqline[0], "POST\0", 5) == 0)
    {
        reqline[1] = strtok (NULL, " \t");
        reqline[2] = strtok (NULL, " \t\n");
        if ( strncmp( reqline[2], "HTTP/1.1", 8) != 0 && strncmp( reqline[2], "HTTP/1.1", 8)!=0 )
        {
            write(clients[n], "HTTP/1.1 400 Bad Request\n", 25);
        }
        else
        {
            if ( strncmp(reqline[1], "/\0", 2)==0 )
            {
                //Because if no file is specified, imagen.jpg will be opened by default 
                reqline[1] = "/imagen.jpg";        
            }

            if ( isLightPost(reqline[1]) )
            {
            	printf("\n*********Updating Lights**********\n");
                reqline[1] = "/casa.txt";  
            }

            if ( !strcmp(reqline[1], "/imagen.jpg") )
            {
            	//Calls the function that takes picture
                system("fswebcam -i 0 -d v4l2:/dev/video0 --jpeg 95 --save imagen.jpg -S 20 -r 640x480");
            }

            if ( !strcmp(reqline[1], "/casa.txt") )
            {
                //Update doors' info
                updateDoors();
            }

            strcpy(path, ROOT);
            strcpy(&path[strlen(ROOT)], reqline[1]);
            printf("file: %s\n", path);

            if ( (fd = open(path, O_RDONLY)) != -1 )    //FILE FOUND
            {
                send(clients[n], "HTTP/1.1 200 OK\n\n", 17, 0);
                while ( (bytes_read=read(fd, data_to_send, BYTES)) > 0 )
                    write (clients[n], data_to_send, bytes_read);
            }
            else {
                write(clients[n], "HTTP/1.1 400 Bad Request\n", 25);
            }
        }
    }
    else{}

    //Closing SOCKET
    shutdown ( clients[n], SHUT_RDWR);     
    close( clients[n] );
    clients[n] = -1;
}

int isLightPost(char *input)
{

    char *all = strtok(input, "/");
    char *lights[5];
    all = strtok(all, "_");
    int ctdr = 0;
    while( all != NULL )
    {
    	char *light = all;
        if ( !strcmp(light, "true") || !strcmp(light, "false") )
        {
        	lights[ctdr] = light;
        	all = strtok(NULL, "_");
        	ctdr++;
        }
        else 
        {
            return 0;
        }
    }
    //Here, change the lights' state**************************
    if ( !strcmp(lights[0], "true") )
        lightOn(PIN_LIGHT_1);
    else
        lightOff(PIN_LIGHT_1);
    if ( !strcmp(lights[1], "true") )
        lightOn(PIN_LIGHT_2);
    else
        lightOff(PIN_LIGHT_2);
    if ( !strcmp(lights[2], "true") )
        lightOn(PIN_LIGHT_3);
    else
        lightOff(PIN_LIGHT_3);
    if ( !strcmp(lights[3], "true") )
        lightOn(PIN_LIGHT_4);
    else
        lightOff(PIN_LIGHT_4);
    if ( !strcmp(lights[4], "true") )
        lightOn(PIN_LIGHT_5);
    else
        lightOff(PIN_LIGHT_5);
    //********************************************************

    //Edit the file casa.txt
    char const *fileName = "casa.txt";
    FILE *file = fopen(fileName, "r");
    char line[256];
    char *houseInfo[9];
    while (fgets(line, sizeof(line), file)) 
    {
        //Starts read the line
        char* textLine = line;
        textLine = strtok(textLine, ".");
        int ctdr = 0;
        while( textLine != NULL )
        {
            char *info = textLine;
            if (ctdr < 5)
                houseInfo[ctdr] = lights[ctdr];
            else
                houseInfo[ctdr] = info;
            textLine = strtok(NULL, ".");
            ctdr++;
        }

    }
    fclose(file);
    file = fopen(fileName, "w");
    fprintf(file, "%s.", houseInfo[0]);
    fprintf(file, "%s.", houseInfo[1]);
    fprintf(file, "%s.", houseInfo[2]);
    fprintf(file, "%s.", houseInfo[3]);
    fprintf(file, "%s.", houseInfo[4]);
    fprintf(file, "%s.", houseInfo[5]);
    fprintf(file, "%s.", houseInfo[6]);
    fprintf(file, "%s.", houseInfo[7]);
    fprintf(file, "%s.", houseInfo[8]);
    fclose(file);  
    return 1;
}

void updateDoors()
{
    //Edit the file casa.txt
    char const *fileName = "casa.txt";
    FILE *file = fopen(fileName, "r");
    char line[256];
    char *houseInfo[9];
    while (fgets(line, sizeof(line), file)) 
    {
        //Starts read the line
        char* textLine = line;
        textLine = strtok(textLine, ".");
        int ctdr = 0;
        while( textLine != NULL )
        {
            char *info = textLine;
            houseInfo[ctdr] = info;
            textLine = strtok(NULL, ".");
            ctdr++;
        }

    }
    fclose(file);

    printf("********UPDATING DOORS*********");
    //Reading all doors*****************************
    if ( readDoor(PIN_DOOR_IN_1) )
        houseInfo[5] = "true";
    else
        houseInfo[5] = "false";

    if ( readDoor(PIN_DOOR_IN_2) )
        houseInfo[6] = "true";
    else
        houseInfo[6] = "false";

    if ( readDoor(PIN_DOOR_IN_3) )
        houseInfo[7] = "true";
    else
        houseInfo[7] = "false";

    if ( readDoor(PIN_DOOR_IN_4) )
        houseInfo[8] = "true";
    else
        houseInfo[8] = "false";
    //*********************************************

    
    file = fopen(fileName, "w");
    fprintf(file, "%s.", houseInfo[0]);
    fprintf(file, "%s.", houseInfo[1]);
    fprintf(file, "%s.", houseInfo[2]);
    fprintf(file, "%s.", houseInfo[3]);
    fprintf(file, "%s.", houseInfo[4]);
    fprintf(file, "%s.", houseInfo[5]);
    fprintf(file, "%s.", houseInfo[6]);
    fprintf(file, "%s.", houseInfo[7]);
    fprintf(file, "%s.", houseInfo[8]);
    fclose(file);
}
