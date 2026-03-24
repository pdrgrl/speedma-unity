/* Initialization */
#include "SimpleSwitchTest_model.h"
#include "SimpleSwitchTest_11mix.h"
#include "SimpleSwitchTest_12jac.h"
#if defined(__cplusplus)
extern "C" {
#endif

void SimpleSwitchTest_functionInitialEquations_0(DATA *data, threadData_t *threadData);

/*
equation index: 1
type: SIMPLE_ASSIGN
Ground.p.i = 0.0
*/
void SimpleSwitchTest_eqFunction_1(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,1};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[3]] /* Ground.p.i variable */) = 0.0;
  threadData->lastEquationSolved = 1;
}

/*
equation index: 2
type: SIMPLE_ASSIGN
Resistor.R_actual = Resistor.R * (1.0 + Resistor.alpha * (Resistor.T - Resistor.T_ref))
*/
void SimpleSwitchTest_eqFunction_2(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,2};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[6]] /* Resistor.R_actual variable */) = ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[2]] /* Resistor.R PARAM */)) * (1.0 + ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[6]] /* Resistor.alpha PARAM */)) * ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[3]] /* Resistor.T PARAM */) - (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[5]] /* Resistor.T_ref PARAM */)));
  threadData->lastEquationSolved = 2;
}

/*
equation index: 3
type: SIMPLE_ASSIGN
$outputAlias_outputVoltage = if switchOn then BooleanToRealConverter.realTrue else BooleanToRealConverter.realFalse
*/
void SimpleSwitchTest_eqFunction_3(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,3};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[1]] /* $outputAlias_outputVoltage DUMMY_STATE */) = ((data->localData[0]->booleanVars[data->simulationInfo->booleanVarsIndex[0]] /* switchOn variable */)?(data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[1]] /* BooleanToRealConverter.realTrue PARAM */):(data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[0]] /* BooleanToRealConverter.realFalse PARAM */));
  threadData->lastEquationSolved = 3;
}

/*
equation index: 4
type: SIMPLE_ASSIGN
outputVoltage = $outputAlias_outputVoltage
*/
void SimpleSwitchTest_eqFunction_4(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,4};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[10]] /* outputVoltage variable */) = (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[1]] /* $outputAlias_outputVoltage DUMMY_STATE */);
  threadData->lastEquationSolved = 4;
}

/*
equation index: 5
type: SIMPLE_ASSIGN
Resistor.i = $outputAlias_outputVoltage / Resistor.R_actual
*/
void SimpleSwitchTest_eqFunction_5(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,5};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[7]] /* Resistor.i variable */) = DIVISION_SIM((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[1]] /* $outputAlias_outputVoltage DUMMY_STATE */),(data->localData[0]->realVars[data->simulationInfo->realVarsIndex[6]] /* Resistor.R_actual variable */),"Resistor.R_actual",equationIndexes);
  threadData->lastEquationSolved = 5;
}

/*
equation index: 6
type: SIMPLE_ASSIGN
Resistor.LossPower = $outputAlias_outputVoltage * Resistor.i
*/
void SimpleSwitchTest_eqFunction_6(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,6};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[5]] /* Resistor.LossPower variable */) = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[1]] /* $outputAlias_outputVoltage DUMMY_STATE */)) * ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[7]] /* Resistor.i variable */));
  threadData->lastEquationSolved = 6;
}

/*
equation index: 7
type: SIMPLE_ASSIGN
Ground.p.v = 0.0
*/
void SimpleSwitchTest_eqFunction_7(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,7};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[4]] /* Ground.p.v variable */) = 0.0;
  threadData->lastEquationSolved = 7;
}

/*
equation index: 8
type: SIMPLE_ASSIGN
Sensor.p.i = 0.0
*/
void SimpleSwitchTest_eqFunction_8(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,8};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[9]] /* Sensor.p.i variable */) = 0.0;
  threadData->lastEquationSolved = 8;
}

/*
equation index: 9
type: SIMPLE_ASSIGN
Sensor.n.i = 0.0
*/
void SimpleSwitchTest_eqFunction_9(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,9};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[8]] /* Sensor.n.i variable */) = 0.0;
  threadData->lastEquationSolved = 9;
}
extern void SimpleSwitchTest_eqFunction_12(DATA *data, threadData_t *threadData);


/*
equation index: 11
type: SIMPLE_ASSIGN
$DER.$outputAlias_outputVoltage = $outputVoltage_der
*/
void SimpleSwitchTest_eqFunction_11(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,11};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[0]] /* der($outputAlias_outputVoltage) DUMMY_DER */) = (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[2]] /* $outputVoltage_der variable */);
  threadData->lastEquationSolved = 11;
}
OMC_DISABLE_OPT
void SimpleSwitchTest_functionInitialEquations_0(DATA *data, threadData_t *threadData)
{
  static void (*const eqFunctions[11])(DATA*, threadData_t*) = {
    SimpleSwitchTest_eqFunction_1,
    SimpleSwitchTest_eqFunction_2,
    SimpleSwitchTest_eqFunction_3,
    SimpleSwitchTest_eqFunction_4,
    SimpleSwitchTest_eqFunction_5,
    SimpleSwitchTest_eqFunction_6,
    SimpleSwitchTest_eqFunction_7,
    SimpleSwitchTest_eqFunction_8,
    SimpleSwitchTest_eqFunction_9,
    SimpleSwitchTest_eqFunction_12,
    SimpleSwitchTest_eqFunction_11
  };
  
  for (int id = 0; id < 11; id++) {
    eqFunctions[id](data, threadData);
  }
}

int SimpleSwitchTest_functionInitialEquations(DATA *data, threadData_t *threadData)
{
  data->simulationInfo->discreteCall = 1;
  SimpleSwitchTest_functionInitialEquations_0(data, threadData);
  data->simulationInfo->discreteCall = 0;
  
  return 0;
}

/* No SimpleSwitchTest_functionInitialEquations_lambda0 function */

int SimpleSwitchTest_functionRemovedInitialEquations(DATA *data, threadData_t *threadData)
{
  const int *equationIndexes = NULL;
  double res = 0.0;

  
  return 0;
}


#if defined(__cplusplus)
}
#endif
