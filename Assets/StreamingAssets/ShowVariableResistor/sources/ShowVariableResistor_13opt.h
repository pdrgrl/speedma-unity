#if defined(__cplusplus)
  extern "C" {
#endif
  int ShowVariableResistor_mayer(DATA* data, modelica_real** res, short*);
  int ShowVariableResistor_lagrange(DATA* data, modelica_real** res, short *, short *);
  int ShowVariableResistor_getInputVarIndicesInOptimization(DATA* data, int* input_var_indices);
  int ShowVariableResistor_pickUpBoundsForInputsInOptimization(DATA* data, modelica_real* min, modelica_real* max, modelica_real*nominal, modelica_boolean *useNominal, char ** name, modelica_real * start, modelica_real * startTimeOpt);
  int ShowVariableResistor_setInputData(DATA *data, const modelica_boolean file);
  int ShowVariableResistor_getTimeGrid(DATA *data, modelica_integer * nsi, modelica_real**t);
#if defined(__cplusplus)
}
#endif
