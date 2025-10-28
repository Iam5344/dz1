#include <iostream>
#include <string>
using namespace std;

// Спадкування
class Animal {
public:
    string name;
    
    Animal(string n) {
        name = n;
    }
    
    void eat() {
        cout << name << " ест" << endl;
    }
};

class Dog : public Animal {
public:
    Dog(string n) : Animal(n) {}
    
    void bark() {
        cout << name << " лает" << endl;
    }
};

// Композиція
class Engine {
public:
    int power;
    
    Engine(int p) {
        power = p;
    }
    
    void start() {
        cout << "Двигатель запущен" << endl;
    }
};

class Car {
private:
    Engine engine;
    
public:
    string model;
    
    Car(string m, int p) : engine(p) {
        model = m;
    }
    
    void drive() {
        engine.start();
        cout << model << " едет" << endl;
    }
};

// Агрегація
class Teacher {
public:
    string name;
    
    Teacher(string n) {
        name = n;
    }
};

class University {
private:
    Teacher* teacher;
    
public:
    string universityName;
    
    University(string n) {
        universityName = n;
        teacher = nullptr;
    }
    
    void setTeacher(Teacher* t) {
        teacher = t;
    }
    
    void showTeacher() {
        if (teacher != nullptr) {
            cout << "Преподаватель: " << teacher->name << endl;
        }
    }
};

int main() {
    Dog dog("Бобик");
    dog.eat();
    dog.bark();
    
    Car car("Toyota", 150);
    car.drive();
    
    Teacher teacher("Иванов");
    University university("КПИ");
    university.setTeacher(&teacher);
    university.showTeacher();
    
    return 0;
}
