#include <stdio.h> 
 #include <stdlib.h> 
 #include <stdbool.h> 
 #include <string.h> 
 #define assertEqual(cond) do { if (!(cond)) { fprintf(stderr, "Test Failed :'%s' not correct", #cond); return;  } } while (0) 
 bool isPalindrome(const char *str) {int len = strlen(str); int i, j;for (i = 0, j = len - 1; i < j; i++, j--){if (str[i] != str[j]) {return false;}}return true;}void Test(){assertEqual(isPalindrome("level") == true);assertEqual(isPalindrome("hello") == false);assertEqual(isPalindrome("radar") == true);assertEqual(isPalindrome("A man, a plan, a canal, Panama") == false); }int main() {Test();return 0;}