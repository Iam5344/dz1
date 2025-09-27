#include <iostream>
using namespace std;

class String {
private:
    char* data;
    size_t size;

public:
    String() : data(nullptr), size(0) {
        data = new char[1];
        data[0] = '\0';
    }

    String(const char* str) {
        if (str) {
            size = 0;
            while (str[size]) size++;
            
            data = new char[size + 1];
            for (size_t i = 0; i <= size; i++) {
                data[i] = str[i];
            }
        } else {
            size = 0;
            data = new char[1];
            data[0] = '\0';
        }
    }

    String() {
        delete[] data;
    }

    // Перевантаження оператору []
    char& operator[](size_t index) {
        return data[index];
    }

    const char& operator[](size_t index) const {
        return data[index];
    }

    friend ostream& operator<<(ostream& os, const String& str) {
        if (str.data) {
            os << str.data;
        }
        return os;
    }

    size_t length() const { return size; }
};

int main() {
    setlocale(0,"");
    String s("Hello");
    
    cout << "s[0] = " << s[0] << endl;
    cout << "s[1] = " << s[1] << endl;
    
    s[0] = 'h';
    cout << "Після зміни: " << s << endl;
    
    return 0;
}
