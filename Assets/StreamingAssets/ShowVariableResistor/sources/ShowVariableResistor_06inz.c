/* Initialization */
#include "ShowVariableResistor_model.h"
#include "ShowVariableResistor_11mix.h"
#include "ShowVariableResistor_12jac.h"
#if defined(__cplusplus)
extern "C" {
#endif

void ShowVariableResistor_functionInitialEquations_0(DATA *data, threadData_t *threadData);

/*
equation index: 1
type: SIMPLE_ASSIGN
R5.R_actual = R5.R * (1.0 + R5.alpha * (R5.T - R5.T_ref))
*/
void ShowVariableResistor_eqFunction_1(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,1};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[19]] /* R5.R_actual variable */) = ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[20]] /* R5.R PARAM */)) * (1.0 + ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[24]] /* R5.alpha PARAM */)) * ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[21]] /* R5.T PARAM */) - (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[23]] /* R5.T_ref PARAM */)));
  threadData->lastEquationSolved = 1;
}

/*
equation index: 2
type: SIMPLE_ASSIGN
R4.R_actual = R4.R * (1.0 + R4.alpha * (R4.T - R4.T_ref))
*/
void ShowVariableResistor_eqFunction_2(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,2};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[15]] /* R4.R_actual variable */) = ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[15]] /* R4.R PARAM */)) * (1.0 + ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[19]] /* R4.alpha PARAM */)) * ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[16]] /* R4.T PARAM */) - (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[18]] /* R4.T_ref PARAM */)));
  threadData->lastEquationSolved = 2;
}

/*
equation index: 3
type: SIMPLE_ASSIGN
R3.R_actual = R3.R * (1.0 + R3.alpha * (R3.T - R3.T_ref))
*/
void ShowVariableResistor_eqFunction_3(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,3};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[11]] /* R3.R_actual variable */) = ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[10]] /* R3.R PARAM */)) * (1.0 + ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[14]] /* R3.alpha PARAM */)) * ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[11]] /* R3.T PARAM */) - (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[13]] /* R3.T_ref PARAM */)));
  threadData->lastEquationSolved = 3;
}

/*
equation index: 4
type: SIMPLE_ASSIGN
R2.R_actual = R2.R * (1.0 + R2.alpha * (R2.T - R2.T_ref))
*/
void ShowVariableResistor_eqFunction_4(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,4};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[8]] /* R2.R_actual variable */) = ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[5]] /* R2.R PARAM */)) * (1.0 + ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[9]] /* R2.alpha PARAM */)) * ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[6]] /* R2.T PARAM */) - (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[8]] /* R2.T_ref PARAM */)));
  threadData->lastEquationSolved = 4;
}

/*
equation index: 5
type: SIMPLE_ASSIGN
R1.R_actual = R1.R * (1.0 + R1.alpha * (R1.T - R1.T_ref))
*/
void ShowVariableResistor_eqFunction_5(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,5};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[4]] /* R1.R_actual variable */) = ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[0]] /* R1.R PARAM */)) * (1.0 + ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[4]] /* R1.alpha PARAM */)) * ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[1]] /* R1.T PARAM */) - (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[3]] /* R1.T_ref PARAM */)));
  threadData->lastEquationSolved = 5;
}
extern void ShowVariableResistor_eqFunction_40(DATA *data, threadData_t *threadData);

extern void ShowVariableResistor_eqFunction_41(DATA *data, threadData_t *threadData);


/*
equation index: 18
type: LINEAR

<var>R5.i</var>
<row>

</row>
<matrix>
</matrix>
*/
OMC_DISABLE_OPT
void ShowVariableResistor_eqFunction_18(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,18};
  /* Linear equation system */
  int retValue;
  double aux_x[1] = { (data->localData[1]->realVars[data->simulationInfo->realVarsIndex[20]] /* R5.i variable */) };
  infoStreamPrint(OMC_LOG_DT, 0, "Solving linear system 18 (STRICT TEARING SET if tearing enabled) at time = %18.10e", data->localData[0]->timeValue);

  retValue = solve_linear_system(data, threadData, 0, &aux_x[0]);

  /* check if solution process was successful */
  if (retValue > 0){
    const int indexes[2] = {1,18};
    throwStreamPrintWithEquationIndexes(threadData, omc_dummyFileInfo, indexes, "Solving linear system 18 failed at time=%.15g.\nFor more information please use -lv LOG_LS.", data->localData[0]->timeValue);
  }
  /* write solution */
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[20]] /* R5.i variable */) = aux_x[0];

  threadData->lastEquationSolved = 18;
}
extern void ShowVariableResistor_eqFunction_71(DATA *data, threadData_t *threadData);

extern void ShowVariableResistor_eqFunction_67(DATA *data, threadData_t *threadData);

extern void ShowVariableResistor_eqFunction_68(DATA *data, threadData_t *threadData);


/*
equation index: 32
type: LINEAR

<var>R3.i</var>
<row>

</row>
<matrix>
</matrix>
*/
OMC_DISABLE_OPT
void ShowVariableResistor_eqFunction_32(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,32};
  /* Linear equation system */
  int retValue;
  double aux_x[1] = { (data->localData[1]->realVars[data->simulationInfo->realVarsIndex[12]] /* R3.i variable */) };
  infoStreamPrint(OMC_LOG_DT, 0, "Solving linear system 32 (STRICT TEARING SET if tearing enabled) at time = %18.10e", data->localData[0]->timeValue);

  retValue = solve_linear_system(data, threadData, 1, &aux_x[0]);

  /* check if solution process was successful */
  if (retValue > 0){
    const int indexes[2] = {1,32};
    throwStreamPrintWithEquationIndexes(threadData, omc_dummyFileInfo, indexes, "Solving linear system 32 failed at time=%.15g.\nFor more information please use -lv LOG_LS.", data->localData[0]->timeValue);
  }
  /* write solution */
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[12]] /* R3.i variable */) = aux_x[0];

  threadData->lastEquationSolved = 32;
}
extern void ShowVariableResistor_eqFunction_55(DATA *data, threadData_t *threadData);

extern void ShowVariableResistor_eqFunction_54(DATA *data, threadData_t *threadData);

extern void ShowVariableResistor_eqFunction_53(DATA *data, threadData_t *threadData);


/*
equation index: 36
type: SIMPLE_ASSIGN
Ground2.p.i = R5.i + R3.i
*/
void ShowVariableResistor_eqFunction_36(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,36};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[1]] /* Ground2.p.i variable */) = (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[20]] /* R5.i variable */) + (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[12]] /* R3.i variable */);
  threadData->lastEquationSolved = 36;
}

/*
equation index: 37
type: SIMPLE_ASSIGN
SineVoltage1.i = Ground2.p.i
*/
void ShowVariableResistor_eqFunction_37(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,37};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[22]] /* SineVoltage1.i variable */) = (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[1]] /* Ground2.p.i variable */);
  threadData->lastEquationSolved = 37;
}

/*
equation index: 38
type: SIMPLE_ASSIGN
Ground1.p.v = 0.0
*/
void ShowVariableResistor_eqFunction_38(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,38};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[0]] /* Ground1.p.v variable */) = 0.0;
  threadData->lastEquationSolved = 38;
}

/*
equation index: 39
type: SIMPLE_ASSIGN
Ground2.p.v = 0.0
*/
void ShowVariableResistor_eqFunction_39(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,39};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[2]] /* Ground2.p.v variable */) = 0.0;
  threadData->lastEquationSolved = 39;
}
OMC_DISABLE_OPT
void ShowVariableResistor_functionInitialEquations_0(DATA *data, threadData_t *threadData)
{
  static void (*const eqFunctions[19])(DATA*, threadData_t*) = {
    ShowVariableResistor_eqFunction_1,
    ShowVariableResistor_eqFunction_2,
    ShowVariableResistor_eqFunction_3,
    ShowVariableResistor_eqFunction_4,
    ShowVariableResistor_eqFunction_5,
    ShowVariableResistor_eqFunction_40,
    ShowVariableResistor_eqFunction_41,
    ShowVariableResistor_eqFunction_18,
    ShowVariableResistor_eqFunction_71,
    ShowVariableResistor_eqFunction_67,
    ShowVariableResistor_eqFunction_68,
    ShowVariableResistor_eqFunction_32,
    ShowVariableResistor_eqFunction_55,
    ShowVariableResistor_eqFunction_54,
    ShowVariableResistor_eqFunction_53,
    ShowVariableResistor_eqFunction_36,
    ShowVariableResistor_eqFunction_37,
    ShowVariableResistor_eqFunction_38,
    ShowVariableResistor_eqFunction_39
  };
  
  for (int id = 0; id < 19; id++) {
    eqFunctions[id](data, threadData);
  }
}

int ShowVariableResistor_functionInitialEquations(DATA *data, threadData_t *threadData)
{
  data->simulationInfo->discreteCall = 1;
  ShowVariableResistor_functionInitialEquations_0(data, threadData);
  data->simulationInfo->discreteCall = 0;
  
  return 0;
}

/* No ShowVariableResistor_functionInitialEquations_lambda0 function */

int ShowVariableResistor_functionRemovedInitialEquations(DATA *data, threadData_t *threadData)
{
  const int *equationIndexes = NULL;
  double res = 0.0;

  
  return 0;
}


#if defined(__cplusplus)
}
#endif
