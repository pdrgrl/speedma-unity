/* Linear Systems */
#include "ShowVariableResistor_model.h"
#include "ShowVariableResistor_12jac.h"
#if defined(__cplusplus)
extern "C" {
#endif

/* linear systems */

/*
equation index: 56
type: SIMPLE_ASSIGN
VariableResistor.v = VariableResistor.R_actual * R5.i
*/
void ShowVariableResistor_eqFunction_56(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,56};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[26]] /* VariableResistor.v variable */) = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[25]] /* VariableResistor.R_actual variable */)) * ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[20]] /* R5.i variable */));
  threadData->lastEquationSolved = 56;
}
/*
equation index: 57
type: SIMPLE_ASSIGN
R5.v = R5.R_actual * R5.i
*/
void ShowVariableResistor_eqFunction_57(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,57};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[21]] /* R5.v variable */) = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[19]] /* R5.R_actual variable */)) * ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[20]] /* R5.i variable */));
  threadData->lastEquationSolved = 57;
}
/*
equation index: 58
type: SIMPLE_ASSIGN
R4.v = R4.R_actual * R5.i
*/
void ShowVariableResistor_eqFunction_58(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,58};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[17]] /* R4.v variable */) = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[15]] /* R4.R_actual variable */)) * ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[20]] /* R5.i variable */));
  threadData->lastEquationSolved = 58;
}
/*
equation index: 59
type: SIMPLE_ASSIGN
R4.n.v = R5.v + VariableResistor.v
*/
void ShowVariableResistor_eqFunction_59(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,59};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[16]] /* R4.n.v variable */) = (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[21]] /* R5.v variable */) + (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[26]] /* VariableResistor.v variable */);
  threadData->lastEquationSolved = 59;
}

void residualFunc66(RESIDUAL_USERDATA* userData, const double* xloc, double* res, const int* iflag)
{
  DATA *data = userData->data;
  threadData_t *threadData = userData->threadData;
  const int equationIndexes[2] = {1,66};
  JACOBIAN* jacobian = NULL;
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[20]] /* R5.i variable */) = xloc[0];
  /* local constraints */
  ShowVariableResistor_eqFunction_56(data, threadData);
  /* local constraints */
  ShowVariableResistor_eqFunction_57(data, threadData);
  /* local constraints */
  ShowVariableResistor_eqFunction_58(data, threadData);
  /* local constraints */
  ShowVariableResistor_eqFunction_59(data, threadData);
  res[0] = (-(data->localData[0]->realVars[data->simulationInfo->realVarsIndex[23]] /* SineVoltage1.v variable */)) - (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[17]] /* R4.v variable */) - (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[16]] /* R4.n.v variable */);
  threadData->lastEquationSolved = 60;
  threadData->lastEquationSolved = 66;
}
OMC_DISABLE_OPT
void initializeStaticLSData66(DATA* data, threadData_t* threadData, LINEAR_SYSTEM_DATA* linearSystemData, modelica_boolean initSparsePattern)
{
  const int indices[1] = {
    20 /* R5.i */
  };
  for (int i = 0; i < 1; ++i) {
    linearSystemData->nominal[i] = data->modelData->realVarsData[indices[i]].attribute.nominal;
    linearSystemData->min[i]     = data->modelData->realVarsData[indices[i]].attribute.min;
    linearSystemData->max[i]     = data->modelData->realVarsData[indices[i]].attribute.max;
  }
}


/*
equation index: 42
type: SIMPLE_ASSIGN
R3.v = R3.R_actual * R3.i
*/
void ShowVariableResistor_eqFunction_42(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,42};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[13]] /* R3.v variable */) = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[11]] /* R3.R_actual variable */)) * ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[12]] /* R3.i variable */));
  threadData->lastEquationSolved = 42;
}
/*
equation index: 43
type: SIMPLE_ASSIGN
R2.v = R2.R_actual * R3.i
*/
void ShowVariableResistor_eqFunction_43(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,43};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[9]] /* R2.v variable */) = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[8]] /* R2.R_actual variable */)) * ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[12]] /* R3.i variable */));
  threadData->lastEquationSolved = 43;
}
/*
equation index: 44
type: SIMPLE_ASSIGN
R1.v = R1.R_actual * R3.i
*/
void ShowVariableResistor_eqFunction_44(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,44};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[6]] /* R1.v variable */) = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[4]] /* R1.R_actual variable */)) * ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[12]] /* R3.i variable */));
  threadData->lastEquationSolved = 44;
}
/*
equation index: 45
type: SIMPLE_ASSIGN
R1.n.v = R3.v + R2.v
*/
void ShowVariableResistor_eqFunction_45(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,45};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[5]] /* R1.n.v variable */) = (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[13]] /* R3.v variable */) + (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[9]] /* R2.v variable */);
  threadData->lastEquationSolved = 45;
}

void residualFunc52(RESIDUAL_USERDATA* userData, const double* xloc, double* res, const int* iflag)
{
  DATA *data = userData->data;
  threadData_t *threadData = userData->threadData;
  const int equationIndexes[2] = {1,52};
  JACOBIAN* jacobian = NULL;
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[12]] /* R3.i variable */) = xloc[0];
  /* local constraints */
  ShowVariableResistor_eqFunction_42(data, threadData);
  /* local constraints */
  ShowVariableResistor_eqFunction_43(data, threadData);
  /* local constraints */
  ShowVariableResistor_eqFunction_44(data, threadData);
  /* local constraints */
  ShowVariableResistor_eqFunction_45(data, threadData);
  res[0] = (-(data->localData[0]->realVars[data->simulationInfo->realVarsIndex[23]] /* SineVoltage1.v variable */)) - (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[6]] /* R1.v variable */) - (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[5]] /* R1.n.v variable */);
  threadData->lastEquationSolved = 46;
  threadData->lastEquationSolved = 52;
}
OMC_DISABLE_OPT
void initializeStaticLSData52(DATA* data, threadData_t* threadData, LINEAR_SYSTEM_DATA* linearSystemData, modelica_boolean initSparsePattern)
{
  const int indices[1] = {
    12 /* R3.i */
  };
  for (int i = 0; i < 1; ++i) {
    linearSystemData->nominal[i] = data->modelData->realVarsData[indices[i]].attribute.nominal;
    linearSystemData->min[i]     = data->modelData->realVarsData[indices[i]].attribute.min;
    linearSystemData->max[i]     = data->modelData->realVarsData[indices[i]].attribute.max;
  }
}


/*
equation index: 22
type: SIMPLE_ASSIGN
R1.v = R1.R_actual * R3.i
*/
void ShowVariableResistor_eqFunction_22(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,22};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[6]] /* R1.v variable */) = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[4]] /* R1.R_actual variable */)) * ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[12]] /* R3.i variable */));
  threadData->lastEquationSolved = 22;
}
/*
equation index: 23
type: SIMPLE_ASSIGN
R2.v = R2.R_actual * R3.i
*/
void ShowVariableResistor_eqFunction_23(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,23};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[9]] /* R2.v variable */) = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[8]] /* R2.R_actual variable */)) * ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[12]] /* R3.i variable */));
  threadData->lastEquationSolved = 23;
}
/*
equation index: 24
type: SIMPLE_ASSIGN
R3.v = R3.R_actual * R3.i
*/
void ShowVariableResistor_eqFunction_24(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,24};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[13]] /* R3.v variable */) = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[11]] /* R3.R_actual variable */)) * ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[12]] /* R3.i variable */));
  threadData->lastEquationSolved = 24;
}
/*
equation index: 25
type: SIMPLE_ASSIGN
R1.n.v = R3.v + R2.v
*/
void ShowVariableResistor_eqFunction_25(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,25};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[5]] /* R1.n.v variable */) = (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[13]] /* R3.v variable */) + (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[9]] /* R2.v variable */);
  threadData->lastEquationSolved = 25;
}

void residualFunc32(RESIDUAL_USERDATA* userData, const double* xloc, double* res, const int* iflag)
{
  DATA *data = userData->data;
  threadData_t *threadData = userData->threadData;
  const int equationIndexes[2] = {1,32};
  JACOBIAN* jacobian = NULL;
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[12]] /* R3.i variable */) = xloc[0];
  /* local constraints */
  ShowVariableResistor_eqFunction_22(data, threadData);
  /* local constraints */
  ShowVariableResistor_eqFunction_23(data, threadData);
  /* local constraints */
  ShowVariableResistor_eqFunction_24(data, threadData);
  /* local constraints */
  ShowVariableResistor_eqFunction_25(data, threadData);
  res[0] = (-(data->localData[0]->realVars[data->simulationInfo->realVarsIndex[23]] /* SineVoltage1.v variable */)) - (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[6]] /* R1.v variable */) - (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[5]] /* R1.n.v variable */);
  threadData->lastEquationSolved = 26;
  threadData->lastEquationSolved = 32;
}
OMC_DISABLE_OPT
void initializeStaticLSData32(DATA* data, threadData_t* threadData, LINEAR_SYSTEM_DATA* linearSystemData, modelica_boolean initSparsePattern)
{
  const int indices[1] = {
    12 /* R3.i */
  };
  for (int i = 0; i < 1; ++i) {
    linearSystemData->nominal[i] = data->modelData->realVarsData[indices[i]].attribute.nominal;
    linearSystemData->min[i]     = data->modelData->realVarsData[indices[i]].attribute.min;
    linearSystemData->max[i]     = data->modelData->realVarsData[indices[i]].attribute.max;
  }
}


/*
equation index: 8
type: SIMPLE_ASSIGN
VariableResistor.v = VariableResistor.R_actual * R5.i
*/
void ShowVariableResistor_eqFunction_8(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,8};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[26]] /* VariableResistor.v variable */) = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[25]] /* VariableResistor.R_actual variable */)) * ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[20]] /* R5.i variable */));
  threadData->lastEquationSolved = 8;
}
/*
equation index: 9
type: SIMPLE_ASSIGN
R5.v = R5.R_actual * R5.i
*/
void ShowVariableResistor_eqFunction_9(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,9};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[21]] /* R5.v variable */) = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[19]] /* R5.R_actual variable */)) * ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[20]] /* R5.i variable */));
  threadData->lastEquationSolved = 9;
}
/*
equation index: 10
type: SIMPLE_ASSIGN
R4.v = R4.R_actual * R5.i
*/
void ShowVariableResistor_eqFunction_10(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,10};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[17]] /* R4.v variable */) = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[15]] /* R4.R_actual variable */)) * ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[20]] /* R5.i variable */));
  threadData->lastEquationSolved = 10;
}
/*
equation index: 11
type: SIMPLE_ASSIGN
R4.n.v = R5.v + VariableResistor.v
*/
void ShowVariableResistor_eqFunction_11(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,11};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[16]] /* R4.n.v variable */) = (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[21]] /* R5.v variable */) + (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[26]] /* VariableResistor.v variable */);
  threadData->lastEquationSolved = 11;
}

void residualFunc18(RESIDUAL_USERDATA* userData, const double* xloc, double* res, const int* iflag)
{
  DATA *data = userData->data;
  threadData_t *threadData = userData->threadData;
  const int equationIndexes[2] = {1,18};
  JACOBIAN* jacobian = NULL;
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[20]] /* R5.i variable */) = xloc[0];
  /* local constraints */
  ShowVariableResistor_eqFunction_8(data, threadData);
  /* local constraints */
  ShowVariableResistor_eqFunction_9(data, threadData);
  /* local constraints */
  ShowVariableResistor_eqFunction_10(data, threadData);
  /* local constraints */
  ShowVariableResistor_eqFunction_11(data, threadData);
  res[0] = (-(data->localData[0]->realVars[data->simulationInfo->realVarsIndex[23]] /* SineVoltage1.v variable */)) - (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[17]] /* R4.v variable */) - (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[16]] /* R4.n.v variable */);
  threadData->lastEquationSolved = 12;
  threadData->lastEquationSolved = 18;
}
OMC_DISABLE_OPT
void initializeStaticLSData18(DATA* data, threadData_t* threadData, LINEAR_SYSTEM_DATA* linearSystemData, modelica_boolean initSparsePattern)
{
  const int indices[1] = {
    20 /* R5.i */
  };
  for (int i = 0; i < 1; ++i) {
    linearSystemData->nominal[i] = data->modelData->realVarsData[indices[i]].attribute.nominal;
    linearSystemData->min[i]     = data->modelData->realVarsData[indices[i]].attribute.min;
    linearSystemData->max[i]     = data->modelData->realVarsData[indices[i]].attribute.max;
  }
}

/* Prototypes for the strict sets (Dynamic Tearing) */

/* Global constraints for the casual sets */
/* function initialize linear systems */
void ShowVariableResistor_initialLinearSystem(int nLinearSystems, LINEAR_SYSTEM_DATA* linearSystemData)
{
  /* linear systems */
  assertStreamPrint(NULL, nLinearSystems > 3, "Internal Error: indexlinearSystem mismatch!");
  linearSystemData[3].equationIndex = 66;
  linearSystemData[3].size = 1;
  linearSystemData[3].nnz = 0;
  linearSystemData[3].method = 1;   /* Symbolic Jacobian available */
  linearSystemData[3].residualFunc = residualFunc66;
  linearSystemData[3].strictTearingFunctionCall = NULL;
  linearSystemData[3].analyticalJacobianColumn = ShowVariableResistor_functionJacLSJac15_column;
  linearSystemData[3].initialAnalyticalJacobian = ShowVariableResistor_initialAnalyticJacobianLSJac15;
  linearSystemData[3].jacobianIndex = 3 /*jacInx*/;
  linearSystemData[3].setA = NULL;  //setLinearMatrixA66;
  linearSystemData[3].setb = NULL;  //setLinearVectorb66;
  linearSystemData[3].initializeStaticLSData = initializeStaticLSData66;
  
  assertStreamPrint(NULL, nLinearSystems > 2, "Internal Error: indexlinearSystem mismatch!");
  linearSystemData[2].equationIndex = 52;
  linearSystemData[2].size = 1;
  linearSystemData[2].nnz = 0;
  linearSystemData[2].method = 1;   /* Symbolic Jacobian available */
  linearSystemData[2].residualFunc = residualFunc52;
  linearSystemData[2].strictTearingFunctionCall = NULL;
  linearSystemData[2].analyticalJacobianColumn = ShowVariableResistor_functionJacLSJac14_column;
  linearSystemData[2].initialAnalyticalJacobian = ShowVariableResistor_initialAnalyticJacobianLSJac14;
  linearSystemData[2].jacobianIndex = 2 /*jacInx*/;
  linearSystemData[2].setA = NULL;  //setLinearMatrixA52;
  linearSystemData[2].setb = NULL;  //setLinearVectorb52;
  linearSystemData[2].initializeStaticLSData = initializeStaticLSData52;
  
  assertStreamPrint(NULL, nLinearSystems > 1, "Internal Error: indexlinearSystem mismatch!");
  linearSystemData[1].equationIndex = 32;
  linearSystemData[1].size = 1;
  linearSystemData[1].nnz = 0;
  linearSystemData[1].method = 1;   /* Symbolic Jacobian available */
  linearSystemData[1].residualFunc = residualFunc32;
  linearSystemData[1].strictTearingFunctionCall = NULL;
  linearSystemData[1].analyticalJacobianColumn = ShowVariableResistor_functionJacLSJac13_column;
  linearSystemData[1].initialAnalyticalJacobian = ShowVariableResistor_initialAnalyticJacobianLSJac13;
  linearSystemData[1].jacobianIndex = 1 /*jacInx*/;
  linearSystemData[1].setA = NULL;  //setLinearMatrixA32;
  linearSystemData[1].setb = NULL;  //setLinearVectorb32;
  linearSystemData[1].initializeStaticLSData = initializeStaticLSData32;
  
  assertStreamPrint(NULL, nLinearSystems > 0, "Internal Error: indexlinearSystem mismatch!");
  linearSystemData[0].equationIndex = 18;
  linearSystemData[0].size = 1;
  linearSystemData[0].nnz = 0;
  linearSystemData[0].method = 1;   /* Symbolic Jacobian available */
  linearSystemData[0].residualFunc = residualFunc18;
  linearSystemData[0].strictTearingFunctionCall = NULL;
  linearSystemData[0].analyticalJacobianColumn = ShowVariableResistor_functionJacLSJac12_column;
  linearSystemData[0].initialAnalyticalJacobian = ShowVariableResistor_initialAnalyticJacobianLSJac12;
  linearSystemData[0].jacobianIndex = 0 /*jacInx*/;
  linearSystemData[0].setA = NULL;  //setLinearMatrixA18;
  linearSystemData[0].setb = NULL;  //setLinearVectorb18;
  linearSystemData[0].initializeStaticLSData = initializeStaticLSData18;
}

#if defined(__cplusplus)
}
#endif
