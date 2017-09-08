#include <Servo.h>

Servo servoMotor;
int servoPin=12;
int value=0;
int comandoAtual = 0;

int alternaVerde = 0;

void setup(){
  Serial.begin(9600);
  // LEDs Vermelhos
  pinMode(7, OUTPUT);
  pinMode(5, OUTPUT);
  // LEDs Verdes
  pinMode(4, OUTPUT);
  pinMode(3, OUTPUT);
}

void loop(){
  if(Serial.available() > 0) 
  {
      value = Serial.parseInt();

      if (value > 0 &&
          value <= 180){
          servoMotor.attach(servoPin);
      
          servoMotor.write(value); 
          Serial.println(value, DEC);          

          delay(800);      
          servoMotor.detach();
      } else {
        // Evita lixo na transmissão
        if (value == 200 ||
            value == 250){
              comandoAtual = value;
            }
      }

      Serial.flush();
  }    

  if (comandoAtual == 200)
  {
      indicarProblema();
  } else if (comandoAtual == 250){
      indicarSemProblema();
  }
}

void indicarProblema(){
  //Serial.println("Problema");

  digitalWrite(4, LOW);
  digitalWrite(3, LOW);
  
  digitalWrite(7, HIGH);
  digitalWrite(5, HIGH);
  delay(250);             
  digitalWrite(7, LOW);   
  digitalWrite(5, LOW); 
  delay(250);            
}


void indicarSemProblema(){
  //Serial.println("Sem Problema");

  if (alternaVerde == 0){
      digitalWrite(4, HIGH);
      digitalWrite(3, LOW);     
      alternaVerde = 1;
  } else {
      digitalWrite(4, LOW);   
      digitalWrite(3, HIGH);       
      alternaVerde = 0;
  }
  
  delay(1000);   
}

