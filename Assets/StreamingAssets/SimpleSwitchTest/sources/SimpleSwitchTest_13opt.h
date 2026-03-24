#if defined(__cplusplus)
  extern "C" {
#endif
  int SimpleSwitchTest_mayer(DATA* data, modelica_real** res, short*);
  int SimpleSwitchTest_lagrange(DATA* data, modelica_real** res, short *, short *);
  int SimpleSwitchTest_getInputVarIndicesInOptimization(DATA* data, int* input_var_indices);
  int SimpleSwitchTest_pickUpBoundsForInputsInOptimization(DATA* data, modelica_real* min, modelica_real* max, modelica_real*nominal, modelica_boolean *useNominal, char ** name, modelica_real * start, modelica_real * startTimeOpt);
  int SimpleSwitchTest_setInputData(DATA *data, const modelica_boolean file);
  int SimpleSwitchTest_getTimeGrid(DATA *data, modelica_integer * nsi, modelica_real**t);
#if defined(__cplusplus)
}
#endif
