 #include <assert.h> 

  void test_addition() { assert(add(1, 2) == 3); assert(add(-1, -2) == -3); assert(add(0, 0) == 0); printf("All test passed"); }  int main(){test_addition(); return 0;} int add(int a, int b) { return a + b; }