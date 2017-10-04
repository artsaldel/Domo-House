cd ~/Escritorio/DomoHouse
echo -e "*********************Aplicando configure*********************\n\n"
./configure --prefix ~/Escritorio/DomoHouse --host arm-linux-gnueabihf --build x86_64-pc-linux-gnu
echo -e "\n\n*********************Construyendo************************\n\n"
make
echo -e "\n\n*********************Instalando*********************\n\n"
make install
echo -e "\n\n*********************Creando empaquetado*********************\n\n"
make dist
echo -e "\n\n*********************Pasando binario a la rasp*********************\n\n"
scp bin/server root@192.168.100.11:~/
echo -e "\n\n*********************Pasando biblioteca de GPIOs*********************\n\n"
scp lib/libConnRasp.so.0 root@192.168.100.11:/usr/lib
echo - "\n\n*********************Pasando biblioteca de servidor*********************\n\n"
scp lib/libFork.so.0 root@192.168.100.11:/usr/lib
echo -e "\n\n*********************Conectando con la rasp*********************\n\n"
ssh root@192.168.100.11