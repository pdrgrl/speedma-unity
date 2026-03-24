/* Jacobians 10 */
#include "ShowVariableResistor_model.h"
#include "ShowVariableResistor_12jac.h"
#include "simulation/jacobian_util.h"
#include "util/omc_file.h"
/* constant equations */
/* dynamic equations */

/*
equation index: 13
type: SIMPLE_ASSIGN
VariableResistor.v.$pDERLSJac12.dummyVarLSJac12 = VariableResistor.R_actual * R5.i.SeedLSJac12
*/
void ShowVariableResistor_eqFunction_13(DATA *data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  const int baseClockIndex = 0;
  const int subClockIndex = 0;
  const int equationIndexes[2] = {1,13};
  jacobian->tmpVars[0] /* VariableResistor.v.$pDERLSJac12.dummyVarLSJac12 JACOBIAN_TMP_VAR */ = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[25]] /* VariableResistor.R_actual variable */)) * (jacobian->seedVars[0] /* R5.i.SeedLSJac12 SEED_VAR */);
  threadData->lastEquationSolved = 13;
}

/*
equation index: 14
type: SIMPLE_ASSIGN
R5.v.$pDERLSJac12.dummyVarLSJac12 = R5.R_actual * R5.i.SeedLSJac12
*/
void ShowVariableResistor_eqFunction_14(DATA *data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  const int baseClockIndex = 0;
  const int subClockIndex = 1;
  const int equationIndexes[2] = {1,14};
  jacobian->tmpVars[1] /* R5.v.$pDERLSJac12.dummyVarLSJac12 JACOBIAN_TMP_VAR */ = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[19]] /* R5.R_actual variable */)) * (jacobian->seedVars[0] /* R5.i.SeedLSJac12 SEED_VAR */);
  threadData->lastEquationSolved = 14;
}

/*
equation index: 15
type: SIMPLE_ASSIGN
R4.v.$pDERLSJac12.dummyVarLSJac12 = R4.R_actual * R5.i.SeedLSJac12
*/
void ShowVariableResistor_eqFunction_15(DATA *data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  const int baseClockIndex = 0;
  const int subClockIndex = 2;
  const int equationIndexes[2] = {1,15};
  jacobian->tmpVars[2] /* R4.v.$pDERLSJac12.dummyVarLSJac12 JACOBIAN_TMP_VAR */ = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[15]] /* R4.R_actual variable */)) * (jacobian->seedVars[0] /* R5.i.SeedLSJac12 SEED_VAR */);
  threadData->lastEquationSolved = 15;
}

/*
equation index: 16
type: SIMPLE_ASSIGN
R4.n.v.$pDERLSJac12.dummyVarLSJac12 = R5.v.$pDERLSJac12.dummyVarLSJac12 + VariableResistor.v.$pDERLSJac12.dummyVarLSJac12
*/
void ShowVariableResistor_eqFunction_16(DATA *data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  const int baseClockIndex = 0;
  const int subClockIndex = 3;
  const int equationIndexes[2] = {1,16};
  jacobian->tmpVars[3] /* R4.n.v.$pDERLSJac12.dummyVarLSJac12 JACOBIAN_TMP_VAR */ = jacobian->tmpVars[1] /* R5.v.$pDERLSJac12.dummyVarLSJac12 JACOBIAN_TMP_VAR */ + jacobian->tmpVars[0] /* VariableResistor.v.$pDERLSJac12.dummyVarLSJac12 JACOBIAN_TMP_VAR */;
  threadData->lastEquationSolved = 16;
}

/*
equation index: 17
type: SIMPLE_ASSIGN
$res_LSJac12_1.$pDERLSJac12.dummyVarLSJac12 = (-R4.v.$pDERLSJac12.dummyVarLSJac12) - R4.n.v.$pDERLSJac12.dummyVarLSJac12
*/
void ShowVariableResistor_eqFunction_17(DATA *data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  const int baseClockIndex = 0;
  const int subClockIndex = 4;
  const int equationIndexes[2] = {1,17};
  jacobian->resultVars[0] /* $res_LSJac12_1.$pDERLSJac12.dummyVarLSJac12 JACOBIAN_VAR */ = (-jacobian->tmpVars[2] /* R4.v.$pDERLSJac12.dummyVarLSJac12 JACOBIAN_TMP_VAR */) - jacobian->tmpVars[3] /* R4.n.v.$pDERLSJac12.dummyVarLSJac12 JACOBIAN_TMP_VAR */;
  threadData->lastEquationSolved = 17;
}

OMC_DISABLE_OPT
int ShowVariableResistor_functionJacLSJac12_constantEqns(DATA* data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  int index = ShowVariableResistor_INDEX_JAC_LSJac12;
  
  
  return 0;
}

int ShowVariableResistor_functionJacLSJac12_column(DATA* data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  int index = ShowVariableResistor_INDEX_JAC_LSJac12;
  
  static void (*const eqFunctions[5])(DATA*, threadData_t*, JACOBIAN*, JACOBIAN*) = {
    ShowVariableResistor_eqFunction_13,
    ShowVariableResistor_eqFunction_14,
    ShowVariableResistor_eqFunction_15,
    ShowVariableResistor_eqFunction_16,
    ShowVariableResistor_eqFunction_17
  };
  
  for (int id = 0; id < 5; id++) {
    eqFunctions[id](data, threadData, jacobian, parentJacobian);
  }
  
  return 0;
}
/* constant equations */
/* dynamic equations */

/*
equation index: 27
type: SIMPLE_ASSIGN
R1.v.$pDERLSJac13.dummyVarLSJac13 = R1.R_actual * R3.i.SeedLSJac13
*/
void ShowVariableResistor_eqFunction_27(DATA *data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  const int baseClockIndex = 0;
  const int subClockIndex = 0;
  const int equationIndexes[2] = {1,27};
  jacobian->tmpVars[0] /* R1.v.$pDERLSJac13.dummyVarLSJac13 JACOBIAN_TMP_VAR */ = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[4]] /* R1.R_actual variable */)) * (jacobian->seedVars[0] /* R3.i.SeedLSJac13 SEED_VAR */);
  threadData->lastEquationSolved = 27;
}

/*
equation index: 28
type: SIMPLE_ASSIGN
R2.v.$pDERLSJac13.dummyVarLSJac13 = R2.R_actual * R3.i.SeedLSJac13
*/
void ShowVariableResistor_eqFunction_28(DATA *data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  const int baseClockIndex = 0;
  const int subClockIndex = 1;
  const int equationIndexes[2] = {1,28};
  jacobian->tmpVars[1] /* R2.v.$pDERLSJac13.dummyVarLSJac13 JACOBIAN_TMP_VAR */ = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[8]] /* R2.R_actual variable */)) * (jacobian->seedVars[0] /* R3.i.SeedLSJac13 SEED_VAR */);
  threadData->lastEquationSolved = 28;
}

/*
equation index: 29
type: SIMPLE_ASSIGN
R3.v.$pDERLSJac13.dummyVarLSJac13 = R3.R_actual * R3.i.SeedLSJac13
*/
void ShowVariableResistor_eqFunction_29(DATA *data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  const int baseClockIndex = 0;
  const int subClockIndex = 2;
  const int equationIndexes[2] = {1,29};
  jacobian->tmpVars[2] /* R3.v.$pDERLSJac13.dummyVarLSJac13 JACOBIAN_TMP_VAR */ = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[11]] /* R3.R_actual variable */)) * (jacobian->seedVars[0] /* R3.i.SeedLSJac13 SEED_VAR */);
  threadData->lastEquationSolved = 29;
}

/*
equation index: 30
type: SIMPLE_ASSIGN
R1.n.v.$pDERLSJac13.dummyVarLSJac13 = R3.v.$pDERLSJac13.dummyVarLSJac13 + R2.v.$pDERLSJac13.dummyVarLSJac13
*/
void ShowVariableResistor_eqFunction_30(DATA *data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  const int baseClockIndex = 0;
  const int subClockIndex = 3;
  const int equationIndexes[2] = {1,30};
  jacobian->tmpVars[3] /* R1.n.v.$pDERLSJac13.dummyVarLSJac13 JACOBIAN_TMP_VAR */ = jacobian->tmpVars[2] /* R3.v.$pDERLSJac13.dummyVarLSJac13 JACOBIAN_TMP_VAR */ + jacobian->tmpVars[1] /* R2.v.$pDERLSJac13.dummyVarLSJac13 JACOBIAN_TMP_VAR */;
  threadData->lastEquationSolved = 30;
}

/*
equation index: 31
type: SIMPLE_ASSIGN
$res_LSJac13_1.$pDERLSJac13.dummyVarLSJac13 = (-R1.v.$pDERLSJac13.dummyVarLSJac13) - R1.n.v.$pDERLSJac13.dummyVarLSJac13
*/
void ShowVariableResistor_eqFunction_31(DATA *data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  const int baseClockIndex = 0;
  const int subClockIndex = 4;
  const int equationIndexes[2] = {1,31};
  jacobian->resultVars[0] /* $res_LSJac13_1.$pDERLSJac13.dummyVarLSJac13 JACOBIAN_VAR */ = (-jacobian->tmpVars[0] /* R1.v.$pDERLSJac13.dummyVarLSJac13 JACOBIAN_TMP_VAR */) - jacobian->tmpVars[3] /* R1.n.v.$pDERLSJac13.dummyVarLSJac13 JACOBIAN_TMP_VAR */;
  threadData->lastEquationSolved = 31;
}

OMC_DISABLE_OPT
int ShowVariableResistor_functionJacLSJac13_constantEqns(DATA* data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  int index = ShowVariableResistor_INDEX_JAC_LSJac13;
  
  
  return 0;
}

int ShowVariableResistor_functionJacLSJac13_column(DATA* data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  int index = ShowVariableResistor_INDEX_JAC_LSJac13;
  
  static void (*const eqFunctions[5])(DATA*, threadData_t*, JACOBIAN*, JACOBIAN*) = {
    ShowVariableResistor_eqFunction_27,
    ShowVariableResistor_eqFunction_28,
    ShowVariableResistor_eqFunction_29,
    ShowVariableResistor_eqFunction_30,
    ShowVariableResistor_eqFunction_31
  };
  
  for (int id = 0; id < 5; id++) {
    eqFunctions[id](data, threadData, jacobian, parentJacobian);
  }
  
  return 0;
}
/* constant equations */
/* dynamic equations */

/*
equation index: 47
type: SIMPLE_ASSIGN
R3.v.$pDERLSJac14.dummyVarLSJac14 = R3.R_actual * R3.i.SeedLSJac14
*/
void ShowVariableResistor_eqFunction_47(DATA *data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  const int baseClockIndex = 0;
  const int subClockIndex = 0;
  const int equationIndexes[2] = {1,47};
  jacobian->tmpVars[0] /* R3.v.$pDERLSJac14.dummyVarLSJac14 JACOBIAN_TMP_VAR */ = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[11]] /* R3.R_actual variable */)) * (jacobian->seedVars[0] /* R3.i.SeedLSJac14 SEED_VAR */);
  threadData->lastEquationSolved = 47;
}

/*
equation index: 48
type: SIMPLE_ASSIGN
R2.v.$pDERLSJac14.dummyVarLSJac14 = R2.R_actual * R3.i.SeedLSJac14
*/
void ShowVariableResistor_eqFunction_48(DATA *data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  const int baseClockIndex = 0;
  const int subClockIndex = 1;
  const int equationIndexes[2] = {1,48};
  jacobian->tmpVars[1] /* R2.v.$pDERLSJac14.dummyVarLSJac14 JACOBIAN_TMP_VAR */ = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[8]] /* R2.R_actual variable */)) * (jacobian->seedVars[0] /* R3.i.SeedLSJac14 SEED_VAR */);
  threadData->lastEquationSolved = 48;
}

/*
equation index: 49
type: SIMPLE_ASSIGN
R1.v.$pDERLSJac14.dummyVarLSJac14 = R1.R_actual * R3.i.SeedLSJac14
*/
void ShowVariableResistor_eqFunction_49(DATA *data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  const int baseClockIndex = 0;
  const int subClockIndex = 2;
  const int equationIndexes[2] = {1,49};
  jacobian->tmpVars[2] /* R1.v.$pDERLSJac14.dummyVarLSJac14 JACOBIAN_TMP_VAR */ = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[4]] /* R1.R_actual variable */)) * (jacobian->seedVars[0] /* R3.i.SeedLSJac14 SEED_VAR */);
  threadData->lastEquationSolved = 49;
}

/*
equation index: 50
type: SIMPLE_ASSIGN
R1.n.v.$pDERLSJac14.dummyVarLSJac14 = R3.v.$pDERLSJac14.dummyVarLSJac14 + R2.v.$pDERLSJac14.dummyVarLSJac14
*/
void ShowVariableResistor_eqFunction_50(DATA *data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  const int baseClockIndex = 0;
  const int subClockIndex = 3;
  const int equationIndexes[2] = {1,50};
  jacobian->tmpVars[3] /* R1.n.v.$pDERLSJac14.dummyVarLSJac14 JACOBIAN_TMP_VAR */ = jacobian->tmpVars[0] /* R3.v.$pDERLSJac14.dummyVarLSJac14 JACOBIAN_TMP_VAR */ + jacobian->tmpVars[1] /* R2.v.$pDERLSJac14.dummyVarLSJac14 JACOBIAN_TMP_VAR */;
  threadData->lastEquationSolved = 50;
}

/*
equation index: 51
type: SIMPLE_ASSIGN
$res_LSJac14_1.$pDERLSJac14.dummyVarLSJac14 = (-R1.v.$pDERLSJac14.dummyVarLSJac14) - R1.n.v.$pDERLSJac14.dummyVarLSJac14
*/
void ShowVariableResistor_eqFunction_51(DATA *data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  const int baseClockIndex = 0;
  const int subClockIndex = 4;
  const int equationIndexes[2] = {1,51};
  jacobian->resultVars[0] /* $res_LSJac14_1.$pDERLSJac14.dummyVarLSJac14 JACOBIAN_VAR */ = (-jacobian->tmpVars[2] /* R1.v.$pDERLSJac14.dummyVarLSJac14 JACOBIAN_TMP_VAR */) - jacobian->tmpVars[3] /* R1.n.v.$pDERLSJac14.dummyVarLSJac14 JACOBIAN_TMP_VAR */;
  threadData->lastEquationSolved = 51;
}

OMC_DISABLE_OPT
int ShowVariableResistor_functionJacLSJac14_constantEqns(DATA* data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  int index = ShowVariableResistor_INDEX_JAC_LSJac14;
  
  
  return 0;
}

int ShowVariableResistor_functionJacLSJac14_column(DATA* data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  int index = ShowVariableResistor_INDEX_JAC_LSJac14;
  
  static void (*const eqFunctions[5])(DATA*, threadData_t*, JACOBIAN*, JACOBIAN*) = {
    ShowVariableResistor_eqFunction_47,
    ShowVariableResistor_eqFunction_48,
    ShowVariableResistor_eqFunction_49,
    ShowVariableResistor_eqFunction_50,
    ShowVariableResistor_eqFunction_51
  };
  
  for (int id = 0; id < 5; id++) {
    eqFunctions[id](data, threadData, jacobian, parentJacobian);
  }
  
  return 0;
}
/* constant equations */
/* dynamic equations */

/*
equation index: 61
type: SIMPLE_ASSIGN
VariableResistor.v.$pDERLSJac15.dummyVarLSJac15 = VariableResistor.R_actual * R5.i.SeedLSJac15
*/
void ShowVariableResistor_eqFunction_61(DATA *data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  const int baseClockIndex = 0;
  const int subClockIndex = 0;
  const int equationIndexes[2] = {1,61};
  jacobian->tmpVars[0] /* VariableResistor.v.$pDERLSJac15.dummyVarLSJac15 JACOBIAN_TMP_VAR */ = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[25]] /* VariableResistor.R_actual variable */)) * (jacobian->seedVars[0] /* R5.i.SeedLSJac15 SEED_VAR */);
  threadData->lastEquationSolved = 61;
}

/*
equation index: 62
type: SIMPLE_ASSIGN
R5.v.$pDERLSJac15.dummyVarLSJac15 = R5.R_actual * R5.i.SeedLSJac15
*/
void ShowVariableResistor_eqFunction_62(DATA *data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  const int baseClockIndex = 0;
  const int subClockIndex = 1;
  const int equationIndexes[2] = {1,62};
  jacobian->tmpVars[1] /* R5.v.$pDERLSJac15.dummyVarLSJac15 JACOBIAN_TMP_VAR */ = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[19]] /* R5.R_actual variable */)) * (jacobian->seedVars[0] /* R5.i.SeedLSJac15 SEED_VAR */);
  threadData->lastEquationSolved = 62;
}

/*
equation index: 63
type: SIMPLE_ASSIGN
R4.v.$pDERLSJac15.dummyVarLSJac15 = R4.R_actual * R5.i.SeedLSJac15
*/
void ShowVariableResistor_eqFunction_63(DATA *data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  const int baseClockIndex = 0;
  const int subClockIndex = 2;
  const int equationIndexes[2] = {1,63};
  jacobian->tmpVars[2] /* R4.v.$pDERLSJac15.dummyVarLSJac15 JACOBIAN_TMP_VAR */ = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[15]] /* R4.R_actual variable */)) * (jacobian->seedVars[0] /* R5.i.SeedLSJac15 SEED_VAR */);
  threadData->lastEquationSolved = 63;
}

/*
equation index: 64
type: SIMPLE_ASSIGN
R4.n.v.$pDERLSJac15.dummyVarLSJac15 = R5.v.$pDERLSJac15.dummyVarLSJac15 + VariableResistor.v.$pDERLSJac15.dummyVarLSJac15
*/
void ShowVariableResistor_eqFunction_64(DATA *data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  const int baseClockIndex = 0;
  const int subClockIndex = 3;
  const int equationIndexes[2] = {1,64};
  jacobian->tmpVars[3] /* R4.n.v.$pDERLSJac15.dummyVarLSJac15 JACOBIAN_TMP_VAR */ = jacobian->tmpVars[1] /* R5.v.$pDERLSJac15.dummyVarLSJac15 JACOBIAN_TMP_VAR */ + jacobian->tmpVars[0] /* VariableResistor.v.$pDERLSJac15.dummyVarLSJac15 JACOBIAN_TMP_VAR */;
  threadData->lastEquationSolved = 64;
}

/*
equation index: 65
type: SIMPLE_ASSIGN
$res_LSJac15_1.$pDERLSJac15.dummyVarLSJac15 = (-R4.v.$pDERLSJac15.dummyVarLSJac15) - R4.n.v.$pDERLSJac15.dummyVarLSJac15
*/
void ShowVariableResistor_eqFunction_65(DATA *data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  const int baseClockIndex = 0;
  const int subClockIndex = 4;
  const int equationIndexes[2] = {1,65};
  jacobian->resultVars[0] /* $res_LSJac15_1.$pDERLSJac15.dummyVarLSJac15 JACOBIAN_VAR */ = (-jacobian->tmpVars[2] /* R4.v.$pDERLSJac15.dummyVarLSJac15 JACOBIAN_TMP_VAR */) - jacobian->tmpVars[3] /* R4.n.v.$pDERLSJac15.dummyVarLSJac15 JACOBIAN_TMP_VAR */;
  threadData->lastEquationSolved = 65;
}

OMC_DISABLE_OPT
int ShowVariableResistor_functionJacLSJac15_constantEqns(DATA* data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  int index = ShowVariableResistor_INDEX_JAC_LSJac15;
  
  
  return 0;
}

int ShowVariableResistor_functionJacLSJac15_column(DATA* data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  int index = ShowVariableResistor_INDEX_JAC_LSJac15;
  
  static void (*const eqFunctions[5])(DATA*, threadData_t*, JACOBIAN*, JACOBIAN*) = {
    ShowVariableResistor_eqFunction_61,
    ShowVariableResistor_eqFunction_62,
    ShowVariableResistor_eqFunction_63,
    ShowVariableResistor_eqFunction_64,
    ShowVariableResistor_eqFunction_65
  };
  
  for (int id = 0; id < 5; id++) {
    eqFunctions[id](data, threadData, jacobian, parentJacobian);
  }
  
  return 0;
}
int ShowVariableResistor_functionJacH_column(DATA* data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  return 0;
}
int ShowVariableResistor_functionJacF_column(DATA* data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  return 0;
}
int ShowVariableResistor_functionJacD_column(DATA* data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  return 0;
}
int ShowVariableResistor_functionJacC_column(DATA* data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  return 0;
}
int ShowVariableResistor_functionJacB_column(DATA* data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  return 0;
}
int ShowVariableResistor_functionJacA_column(DATA* data, threadData_t *threadData, JACOBIAN *jacobian, JACOBIAN *parentJacobian)
{
  return 0;
}

OMC_DISABLE_OPT
int ShowVariableResistor_initialAnalyticJacobianLSJac12(DATA* data, threadData_t *threadData, JACOBIAN *jacobian)
{
  size_t count;

  FILE* pFile = openSparsePatternFile(data, threadData, "ShowVariableResistor_JacLSJac12.bin");
  
  initJacobian(jacobian, 1, 1, 5, ShowVariableResistor_functionJacLSJac12_column, NULL, NULL);
  jacobian->sparsePattern = allocSparsePattern(1, 1, 1);
  jacobian->availability = JACOBIAN_AVAILABLE;
  
  /* read lead index of compressed sparse column */
  count = omc_fread(jacobian->sparsePattern->leadindex, sizeof(unsigned int), 1+1, pFile, FALSE);
  if (count != 1+1) {
    throwStreamPrint(threadData, "Error while reading lead index list of sparsity pattern. Expected %d, got %zu", 1+1, count);
  }
  
  /* read sparse index */
  count = omc_fread(jacobian->sparsePattern->index, sizeof(unsigned int), 1, pFile, FALSE);
  if (count != 1) {
    throwStreamPrint(threadData, "Error while reading row index list of sparsity pattern. Expected %d, got %zu", 1, count);
  }
  
  /* write color array */
  /* color 1 with 1 columns */
  readSparsePatternColor(threadData, pFile, jacobian->sparsePattern->colorCols, 1, 1, 1);
  
  omc_fclose(pFile);
  
  return 0;
}
OMC_DISABLE_OPT
int ShowVariableResistor_initialAnalyticJacobianLSJac13(DATA* data, threadData_t *threadData, JACOBIAN *jacobian)
{
  size_t count;

  FILE* pFile = openSparsePatternFile(data, threadData, "ShowVariableResistor_JacLSJac13.bin");
  
  initJacobian(jacobian, 1, 1, 5, ShowVariableResistor_functionJacLSJac13_column, NULL, NULL);
  jacobian->sparsePattern = allocSparsePattern(1, 1, 1);
  jacobian->availability = JACOBIAN_AVAILABLE;
  
  /* read lead index of compressed sparse column */
  count = omc_fread(jacobian->sparsePattern->leadindex, sizeof(unsigned int), 1+1, pFile, FALSE);
  if (count != 1+1) {
    throwStreamPrint(threadData, "Error while reading lead index list of sparsity pattern. Expected %d, got %zu", 1+1, count);
  }
  
  /* read sparse index */
  count = omc_fread(jacobian->sparsePattern->index, sizeof(unsigned int), 1, pFile, FALSE);
  if (count != 1) {
    throwStreamPrint(threadData, "Error while reading row index list of sparsity pattern. Expected %d, got %zu", 1, count);
  }
  
  /* write color array */
  /* color 1 with 1 columns */
  readSparsePatternColor(threadData, pFile, jacobian->sparsePattern->colorCols, 1, 1, 1);
  
  omc_fclose(pFile);
  
  return 0;
}
OMC_DISABLE_OPT
int ShowVariableResistor_initialAnalyticJacobianLSJac14(DATA* data, threadData_t *threadData, JACOBIAN *jacobian)
{
  size_t count;

  FILE* pFile = openSparsePatternFile(data, threadData, "ShowVariableResistor_JacLSJac14.bin");
  
  initJacobian(jacobian, 1, 1, 5, ShowVariableResistor_functionJacLSJac14_column, NULL, NULL);
  jacobian->sparsePattern = allocSparsePattern(1, 1, 1);
  jacobian->availability = JACOBIAN_AVAILABLE;
  
  /* read lead index of compressed sparse column */
  count = omc_fread(jacobian->sparsePattern->leadindex, sizeof(unsigned int), 1+1, pFile, FALSE);
  if (count != 1+1) {
    throwStreamPrint(threadData, "Error while reading lead index list of sparsity pattern. Expected %d, got %zu", 1+1, count);
  }
  
  /* read sparse index */
  count = omc_fread(jacobian->sparsePattern->index, sizeof(unsigned int), 1, pFile, FALSE);
  if (count != 1) {
    throwStreamPrint(threadData, "Error while reading row index list of sparsity pattern. Expected %d, got %zu", 1, count);
  }
  
  /* write color array */
  /* color 1 with 1 columns */
  readSparsePatternColor(threadData, pFile, jacobian->sparsePattern->colorCols, 1, 1, 1);
  
  omc_fclose(pFile);
  
  return 0;
}
OMC_DISABLE_OPT
int ShowVariableResistor_initialAnalyticJacobianLSJac15(DATA* data, threadData_t *threadData, JACOBIAN *jacobian)
{
  size_t count;

  FILE* pFile = openSparsePatternFile(data, threadData, "ShowVariableResistor_JacLSJac15.bin");
  
  initJacobian(jacobian, 1, 1, 5, ShowVariableResistor_functionJacLSJac15_column, NULL, NULL);
  jacobian->sparsePattern = allocSparsePattern(1, 1, 1);
  jacobian->availability = JACOBIAN_AVAILABLE;
  
  /* read lead index of compressed sparse column */
  count = omc_fread(jacobian->sparsePattern->leadindex, sizeof(unsigned int), 1+1, pFile, FALSE);
  if (count != 1+1) {
    throwStreamPrint(threadData, "Error while reading lead index list of sparsity pattern. Expected %d, got %zu", 1+1, count);
  }
  
  /* read sparse index */
  count = omc_fread(jacobian->sparsePattern->index, sizeof(unsigned int), 1, pFile, FALSE);
  if (count != 1) {
    throwStreamPrint(threadData, "Error while reading row index list of sparsity pattern. Expected %d, got %zu", 1, count);
  }
  
  /* write color array */
  /* color 1 with 1 columns */
  readSparsePatternColor(threadData, pFile, jacobian->sparsePattern->colorCols, 1, 1, 1);
  
  omc_fclose(pFile);
  
  return 0;
}
int ShowVariableResistor_initialAnalyticJacobianH(DATA* data, threadData_t *threadData, JACOBIAN *jacobian)
{
  jacobian->availability = JACOBIAN_NOT_AVAILABLE;
  return 1;
}
int ShowVariableResistor_initialAnalyticJacobianF(DATA* data, threadData_t *threadData, JACOBIAN *jacobian)
{
  jacobian->availability = JACOBIAN_NOT_AVAILABLE;
  return 1;
}
int ShowVariableResistor_initialAnalyticJacobianD(DATA* data, threadData_t *threadData, JACOBIAN *jacobian)
{
  jacobian->availability = JACOBIAN_NOT_AVAILABLE;
  return 1;
}
int ShowVariableResistor_initialAnalyticJacobianC(DATA* data, threadData_t *threadData, JACOBIAN *jacobian)
{
  jacobian->availability = JACOBIAN_NOT_AVAILABLE;
  return 1;
}
int ShowVariableResistor_initialAnalyticJacobianB(DATA* data, threadData_t *threadData, JACOBIAN *jacobian)
{
  jacobian->availability = JACOBIAN_NOT_AVAILABLE;
  return 1;
}
int ShowVariableResistor_initialAnalyticJacobianA(DATA* data, threadData_t *threadData, JACOBIAN *jacobian)
{
  jacobian->availability = JACOBIAN_NOT_AVAILABLE;
  return 1;
}


