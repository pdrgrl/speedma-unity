/* Main Simulation File */

#if defined(__cplusplus)
extern "C" {
#endif

#include "SimpleSwitchTest_model.h"
#include "simulation/solver/events.h"
#include "util/real_array.h"



/* dummy VARINFO and FILEINFO */
const VAR_INFO dummyVAR_INFO = omc_dummyVarInfo;

int SimpleSwitchTest_input_function(DATA *data, threadData_t *threadData)
{
  (data->localData[0]->booleanVars[data->simulationInfo->booleanVarsIndex[0]] /* switchOn variable */) = data->simulationInfo->inputVars[0];
  
  return 0;
}

int SimpleSwitchTest_input_function_init(DATA *data, threadData_t *threadData)
{
  data->simulationInfo->inputVars[0] = data->modelData->booleanVarsData[0].attribute.start;
  
  return 0;
}

int SimpleSwitchTest_input_function_updateStartValues(DATA *data, threadData_t *threadData)
{
  data->modelData->booleanVarsData[0].attribute.start = data->simulationInfo->inputVars[0];
  
  return 0;
}

int SimpleSwitchTest_inputNames(DATA *data, char ** names){
  names[0] = (char *) data->modelData->booleanVarsData[0].info.name;
  
  return 0;
}

int SimpleSwitchTest_data_function(DATA *data, threadData_t *threadData)
{
  return 0;
}

int SimpleSwitchTest_dataReconciliationInputNames(DATA *data, char ** names){
  
  return 0;
}

int SimpleSwitchTest_dataReconciliationUnmeasuredVariables(DATA *data, char ** names)
{
  
  return 0;
}

int SimpleSwitchTest_output_function(DATA *data, threadData_t *threadData)
{
  data->simulationInfo->outputVars[0] = (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[10]] /* outputVoltage variable */);
  
  return 0;
}

int SimpleSwitchTest_setc_function(DATA *data, threadData_t *threadData)
{
  
  return 0;
}

int SimpleSwitchTest_setb_function(DATA *data, threadData_t *threadData)
{
  
  return 0;
}


/*
equation index: 12
type: SIMPLE_ASSIGN
$outputVoltage_der = 0.0
*/
void SimpleSwitchTest_eqFunction_12(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,12};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[2]] /* $outputVoltage_der variable */) = 0.0;
  threadData->lastEquationSolved = 12;
}

/*
equation index: 13
type: SIMPLE_ASSIGN
$DER.$outputAlias_outputVoltage = 0.0
*/
void SimpleSwitchTest_eqFunction_13(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,13};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[0]] /* der($outputAlias_outputVoltage) DUMMY_DER */) = 0.0;
  threadData->lastEquationSolved = 13;
}

/*
equation index: 14
type: SIMPLE_ASSIGN
outputVoltage = if switchOn then BooleanToRealConverter.realTrue else BooleanToRealConverter.realFalse
*/
void SimpleSwitchTest_eqFunction_14(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,14};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[10]] /* outputVoltage variable */) = ((data->localData[0]->booleanVars[data->simulationInfo->booleanVarsIndex[0]] /* switchOn variable */)?(data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[1]] /* BooleanToRealConverter.realTrue PARAM */):(data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[0]] /* BooleanToRealConverter.realFalse PARAM */));
  threadData->lastEquationSolved = 14;
}

/*
equation index: 15
type: SIMPLE_ASSIGN
Resistor.i = outputVoltage / Resistor.R_actual
*/
void SimpleSwitchTest_eqFunction_15(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,15};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[7]] /* Resistor.i variable */) = DIVISION_SIM((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[10]] /* outputVoltage variable */),(data->localData[0]->realVars[data->simulationInfo->realVarsIndex[6]] /* Resistor.R_actual variable */),"Resistor.R_actual",equationIndexes);
  threadData->lastEquationSolved = 15;
}

/*
equation index: 16
type: SIMPLE_ASSIGN
Resistor.LossPower = outputVoltage * Resistor.i
*/
void SimpleSwitchTest_eqFunction_16(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,16};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[5]] /* Resistor.LossPower variable */) = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[10]] /* outputVoltage variable */)) * ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[7]] /* Resistor.i variable */));
  threadData->lastEquationSolved = 16;
}

/*
equation index: 17
type: SIMPLE_ASSIGN
$outputAlias_outputVoltage = outputVoltage
*/
void SimpleSwitchTest_eqFunction_17(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,17};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[1]] /* $outputAlias_outputVoltage DUMMY_STATE */) = (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[10]] /* outputVoltage variable */);
  threadData->lastEquationSolved = 17;
}

/*
equation index: 18
type: ALGORITHM

  assert(1.0 + Resistor.alpha * (Resistor.T - Resistor.T_ref) >= 2.220446049250313e-16, "Temperature outside scope of model!");
*/
void SimpleSwitchTest_eqFunction_18(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,18};
  modelica_boolean tmp0;
  static const MMC_DEFSTRINGLIT(tmp1,35,"Temperature outside scope of model!");
  static int tmp2 = 0;
  {
    tmp0 = GreaterEq(1.0 + ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[6]] /* Resistor.alpha PARAM */)) * ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[3]] /* Resistor.T PARAM */) - (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[5]] /* Resistor.T_ref PARAM */)),2.220446049250313e-16);
    if(!tmp0)
    {
      {
        const char* assert_cond = "(1.0 + Resistor.alpha * (Resistor.T - Resistor.T_ref) >= 2.220446049250313e-16)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",15,3,16,43,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(MMC_REFSTRINGLIT(tmp1)));
          data->simulationInfo->needToReThrow = 1;
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",15,3,16,43,0};
          omc_assert_withEquationIndexes(threadData, info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(MMC_REFSTRINGLIT(tmp1)));
        }
      }
    }
  }
  threadData->lastEquationSolved = 18;
}

OMC_DISABLE_OPT
int SimpleSwitchTest_functionDAE(DATA *data, threadData_t *threadData)
{
  int equationIndexes[1] = {0};
#if !defined(OMC_MINIMAL_RUNTIME)
  if (measure_time_flag) rt_tick(SIM_TIMER_DAE);
#endif

  data->simulationInfo->needToIterate = 0;
  data->simulationInfo->discreteCall = 1;
  SimpleSwitchTest_functionLocalKnownVars(data, threadData);
  static void (*const eqFunctions[7])(DATA*, threadData_t*) = {
    SimpleSwitchTest_eqFunction_12,
    SimpleSwitchTest_eqFunction_13,
    SimpleSwitchTest_eqFunction_14,
    SimpleSwitchTest_eqFunction_15,
    SimpleSwitchTest_eqFunction_16,
    SimpleSwitchTest_eqFunction_17,
    SimpleSwitchTest_eqFunction_18
  };
  
  for (int id = 0; id < 7; id++) {
    eqFunctions[id](data, threadData);
  }
  data->simulationInfo->discreteCall = 0;
  
#if !defined(OMC_MINIMAL_RUNTIME)
  if (measure_time_flag) rt_accumulate(SIM_TIMER_DAE);
#endif
  return 0;
}


int SimpleSwitchTest_functionLocalKnownVars(DATA *data, threadData_t *threadData)
{
  
  return 0;
}


int SimpleSwitchTest_functionODE(DATA *data, threadData_t *threadData)
{
#if !defined(OMC_MINIMAL_RUNTIME)
  if (measure_time_flag) rt_tick(SIM_TIMER_FUNCTION_ODE);
#endif

  
  data->simulationInfo->callStatistics.functionODE++;
  
  SimpleSwitchTest_functionLocalKnownVars(data, threadData);
  /* no ODE systems */

#if !defined(OMC_MINIMAL_RUNTIME)
  if (measure_time_flag) rt_accumulate(SIM_TIMER_FUNCTION_ODE);
#endif

  return 0;
}

/* forward the main in the simulation runtime */
extern int _main_SimulationRuntime(int argc, char **argv, DATA *data, threadData_t *threadData);
extern int _main_OptimizationRuntime(int argc, char **argv, DATA *data, threadData_t *threadData);

#include "SimpleSwitchTest_12jac.h"
#include "SimpleSwitchTest_13opt.h"

struct OpenModelicaGeneratedFunctionCallbacks SimpleSwitchTest_callback = {
  NULL,    /* performSimulation */
  NULL,    /* performQSSSimulation */
  NULL,    /* updateContinuousSystem */
  SimpleSwitchTest_callExternalObjectDestructors,    /* callExternalObjectDestructors */
  NULL,    /* initialNonLinearSystem */
  NULL,    /* initialLinearSystem */
  NULL,    /* initialMixedSystem */
  #if !defined(OMC_NO_STATESELECTION)
  SimpleSwitchTest_initializeStateSets,
  #else
  NULL,
  #endif    /* initializeStateSets */
  SimpleSwitchTest_initializeDAEmodeData,
  SimpleSwitchTest_functionODE,
  SimpleSwitchTest_functionAlgebraics,
  SimpleSwitchTest_functionDAE,
  SimpleSwitchTest_functionLocalKnownVars,
  SimpleSwitchTest_input_function,
  SimpleSwitchTest_input_function_init,
  SimpleSwitchTest_input_function_updateStartValues,
  SimpleSwitchTest_data_function,
  SimpleSwitchTest_output_function,
  SimpleSwitchTest_setc_function,
  SimpleSwitchTest_setb_function,
  SimpleSwitchTest_function_storeDelayed,
  SimpleSwitchTest_function_storeSpatialDistribution,
  SimpleSwitchTest_function_initSpatialDistribution,
  SimpleSwitchTest_updateBoundVariableAttributes,
  SimpleSwitchTest_functionInitialEquations,
  GLOBAL_EQUIDISTANT_HOMOTOPY,
  NULL,
  SimpleSwitchTest_functionRemovedInitialEquations,
  SimpleSwitchTest_updateBoundParameters,
  SimpleSwitchTest_checkForAsserts,
  SimpleSwitchTest_function_ZeroCrossingsEquations,
  SimpleSwitchTest_function_ZeroCrossings,
  SimpleSwitchTest_function_updateRelations,
  SimpleSwitchTest_zeroCrossingDescription,
  SimpleSwitchTest_relationDescription,
  SimpleSwitchTest_function_initSample,
  SimpleSwitchTest_INDEX_JAC_A,
  SimpleSwitchTest_INDEX_JAC_B,
  SimpleSwitchTest_INDEX_JAC_C,
  SimpleSwitchTest_INDEX_JAC_D,
  SimpleSwitchTest_INDEX_JAC_F,
  SimpleSwitchTest_INDEX_JAC_H,
  SimpleSwitchTest_initialAnalyticJacobianA,
  SimpleSwitchTest_initialAnalyticJacobianB,
  SimpleSwitchTest_initialAnalyticJacobianC,
  SimpleSwitchTest_initialAnalyticJacobianD,
  SimpleSwitchTest_initialAnalyticJacobianF,
  SimpleSwitchTest_initialAnalyticJacobianH,
  SimpleSwitchTest_functionJacA_column,
  SimpleSwitchTest_functionJacB_column,
  SimpleSwitchTest_functionJacC_column,
  SimpleSwitchTest_functionJacD_column,
  SimpleSwitchTest_functionJacF_column,
  SimpleSwitchTest_functionJacH_column,
  SimpleSwitchTest_linear_model_frame,
  SimpleSwitchTest_linear_model_datarecovery_frame,
  SimpleSwitchTest_mayer,
  SimpleSwitchTest_lagrange,
  SimpleSwitchTest_getInputVarIndicesInOptimization,
  SimpleSwitchTest_pickUpBoundsForInputsInOptimization,
  SimpleSwitchTest_setInputData,
  SimpleSwitchTest_getTimeGrid,
  SimpleSwitchTest_symbolicInlineSystem,
  SimpleSwitchTest_function_initSynchronous,
  SimpleSwitchTest_function_updateSynchronous,
  SimpleSwitchTest_function_equationsSynchronous,
  SimpleSwitchTest_inputNames,
  SimpleSwitchTest_dataReconciliationInputNames,
  SimpleSwitchTest_dataReconciliationUnmeasuredVariables,
  SimpleSwitchTest_read_simulation_info,
  SimpleSwitchTest_read_input_fmu,
  NULL,
  NULL,
  -1,
  NULL,
  NULL,
  -1

};

#define _OMC_LIT_RESOURCE_0_name_data "Complex"
#define _OMC_LIT_RESOURCE_0_dir_data "C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Complex 4.1.0+maint.om"
static const MMC_DEFSTRINGLIT(_OMC_LIT_RESOURCE_0_name,7,_OMC_LIT_RESOURCE_0_name_data);
static const MMC_DEFSTRINGLIT(_OMC_LIT_RESOURCE_0_dir,77,_OMC_LIT_RESOURCE_0_dir_data);

#define _OMC_LIT_RESOURCE_1_name_data "Modelica"
#define _OMC_LIT_RESOURCE_1_dir_data "C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om"
static const MMC_DEFSTRINGLIT(_OMC_LIT_RESOURCE_1_name,8,_OMC_LIT_RESOURCE_1_name_data);
static const MMC_DEFSTRINGLIT(_OMC_LIT_RESOURCE_1_dir,78,_OMC_LIT_RESOURCE_1_dir_data);

#define _OMC_LIT_RESOURCE_2_name_data "ModelicaServices"
#define _OMC_LIT_RESOURCE_2_dir_data "C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/ModelicaServices 4.1.0+maint.om"
static const MMC_DEFSTRINGLIT(_OMC_LIT_RESOURCE_2_name,16,_OMC_LIT_RESOURCE_2_name_data);
static const MMC_DEFSTRINGLIT(_OMC_LIT_RESOURCE_2_dir,86,_OMC_LIT_RESOURCE_2_dir_data);

#define _OMC_LIT_RESOURCE_3_name_data "SimpleSwitchTest"
#define _OMC_LIT_RESOURCE_3_dir_data "Y:/Downloads"
static const MMC_DEFSTRINGLIT(_OMC_LIT_RESOURCE_3_name,16,_OMC_LIT_RESOURCE_3_name_data);
static const MMC_DEFSTRINGLIT(_OMC_LIT_RESOURCE_3_dir,12,_OMC_LIT_RESOURCE_3_dir_data);

static const MMC_DEFSTRUCTLIT(_OMC_LIT_RESOURCES,8,MMC_ARRAY_TAG) {MMC_REFSTRINGLIT(_OMC_LIT_RESOURCE_0_name), MMC_REFSTRINGLIT(_OMC_LIT_RESOURCE_0_dir), MMC_REFSTRINGLIT(_OMC_LIT_RESOURCE_1_name), MMC_REFSTRINGLIT(_OMC_LIT_RESOURCE_1_dir), MMC_REFSTRINGLIT(_OMC_LIT_RESOURCE_2_name), MMC_REFSTRINGLIT(_OMC_LIT_RESOURCE_2_dir), MMC_REFSTRINGLIT(_OMC_LIT_RESOURCE_3_name), MMC_REFSTRINGLIT(_OMC_LIT_RESOURCE_3_dir)}};
void SimpleSwitchTest_setupDataStruc(DATA *data, threadData_t *threadData)
{
  assertStreamPrint(threadData,0!=data, "Error while initialize Data");
  threadData->localRoots[LOCAL_ROOT_SIMULATION_DATA] = data;
  data->callback = &SimpleSwitchTest_callback;
  OpenModelica_updateUriMapping(threadData, MMC_REFSTRUCTLIT(_OMC_LIT_RESOURCES));
  data->modelData->modelName = "SimpleSwitchTest";
  data->modelData->modelFilePrefix = "SimpleSwitchTest";
  data->modelData->modelFileName = "SimpleSwitchTest.mo";
  data->modelData->resultFileName = NULL;
  data->modelData->modelDir = "Y:/Downloads";
  data->modelData->modelGUID = "{699f9b33-dab8-40e8-980e-0416d44baa7c}";
  data->modelData->initXMLData = NULL;
  data->modelData->modelDataXml.infoXMLData = NULL;
  GC_asprintf(&data->modelData->modelDataXml.fileName, "%s/SimpleSwitchTest_info.json", data->modelData->resourcesDir);
  data->modelData->runTestsuite = 0;
  data->modelData->nStatesArray = 0;
  data->modelData->nDiscreteReal = 0;
  data->modelData->nVariablesRealArray = 11;
  data->modelData->nVariablesIntegerArray = 0;
  data->modelData->nVariablesBooleanArray = 1;
  data->modelData->nVariablesStringArray = 0;
  data->modelData->nParametersRealArray = 7;
  data->modelData->nParametersIntegerArray = 0;
  data->modelData->nParametersBooleanArray = 1;
  data->modelData->nParametersStringArray = 0;
  data->modelData->nParametersReal = 7;
  data->modelData->nParametersInteger = 0;
  data->modelData->nParametersBoolean = 1;
  data->modelData->nParametersString = 0;
  data->modelData->nAliasRealArray = 15;
  data->modelData->nAliasIntegerArray = 0;
  data->modelData->nAliasBooleanArray = 1;
  data->modelData->nAliasStringArray = 0;
  data->modelData->nInputVars = 1;
  data->modelData->nOutputVars = 1;
  data->modelData->nZeroCrossings = 0;
  data->modelData->nSamples = 0;
  data->modelData->nRelations = 0;
  data->modelData->nMathEvents = 0;
  data->modelData->nExtObjs = 0;
  data->modelData->modelDataXml.modelInfoXmlLength = 0;
  data->modelData->modelDataXml.nFunctions = 0;
  data->modelData->modelDataXml.nProfileBlocks = 0;
  data->modelData->modelDataXml.nEquations = 30;
  data->modelData->nMixedSystems = 0;
  data->modelData->nLinearSystems = 0;
  data->modelData->nNonLinearSystems = 0;
  data->modelData->nStateSets = 0;
  data->modelData->nJacobians = 6;
  data->modelData->nOptimizeConstraints = 0;
  data->modelData->nOptimizeFinalConstraints = 0;
  data->modelData->nDelayExpressions = 0;
  data->modelData->nBaseClocks = 0;
  data->modelData->nSpatialDistributions = 0;
  data->modelData->nSensitivityVars = 0;
  data->modelData->nSensitivityParamVars = 0;
  data->modelData->nSetcVars = 0;
  data->modelData->ndataReconVars = 0;
  data->modelData->nSetbVars = 0;
  data->modelData->nRelatedBoundaryConditions = 0;
  data->modelData->linearizationDumpLanguage = OMC_LINEARIZE_DUMP_LANGUAGE_MODELICA;
}

static int rml_execution_failed()
{
  fflush(NULL);
  fprintf(stderr, "Execution failed!\n");
  fflush(NULL);
  return 1;
}

