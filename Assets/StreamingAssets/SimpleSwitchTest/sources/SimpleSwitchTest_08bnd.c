/* update bound parameters and variable attributes (start, nominal, min, max) */
#include "SimpleSwitchTest_model.h"
#if defined(__cplusplus)
extern "C" {
#endif

OMC_DISABLE_OPT
int SimpleSwitchTest_updateBoundVariableAttributes(DATA *data, threadData_t *threadData)
{
  /* min ******************************************************** */
  infoStreamPrint(OMC_LOG_INIT, 1, "updating min-values");
  messageClose(OMC_LOG_INIT);
  
  /* max ******************************************************** */
  infoStreamPrint(OMC_LOG_INIT, 1, "updating max-values");
  messageClose(OMC_LOG_INIT);
  
  /* nominal **************************************************** */
  infoStreamPrint(OMC_LOG_INIT, 1, "updating nominal-values");
  messageClose(OMC_LOG_INIT);
  
  /* start ****************************************************** */
  infoStreamPrint(OMC_LOG_INIT, 1, "updating primary start-values");
  messageClose(OMC_LOG_INIT);
  
  return 0;
}

void SimpleSwitchTest_updateBoundParameters_0(DATA *data, threadData_t *threadData);

/*
equation index: 19
type: SIMPLE_ASSIGN
Resistor.T = Resistor.T_ref
*/
OMC_DISABLE_OPT
static void SimpleSwitchTest_eqFunction_19(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,19};
  (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[3]] /* Resistor.T PARAM */) = (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[5]] /* Resistor.T_ref PARAM */);
  threadData->lastEquationSolved = 19;
}

/*
equation index: 20
type: SIMPLE_ASSIGN
Resistor.T_heatPort = Resistor.T
*/
OMC_DISABLE_OPT
static void SimpleSwitchTest_eqFunction_20(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,20};
  (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[4]] /* Resistor.T_heatPort PARAM */) = (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[3]] /* Resistor.T PARAM */);
  threadData->lastEquationSolved = 20;
}
extern void SimpleSwitchTest_eqFunction_9(DATA *data, threadData_t *threadData);

extern void SimpleSwitchTest_eqFunction_8(DATA *data, threadData_t *threadData);

extern void SimpleSwitchTest_eqFunction_7(DATA *data, threadData_t *threadData);

extern void SimpleSwitchTest_eqFunction_2(DATA *data, threadData_t *threadData);

extern void SimpleSwitchTest_eqFunction_1(DATA *data, threadData_t *threadData);


/*
equation index: 27
type: ALGORITHM

  assert(Resistor.T_ref >= 0.0, "Variable violating min constraint: 0.0 <= Resistor.T_ref, has value: " + String(Resistor.T_ref, "g"));
*/
OMC_DISABLE_OPT
static void SimpleSwitchTest_eqFunction_27(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,27};
  modelica_boolean tmp0;
  static const MMC_DEFSTRINGLIT(tmp1,69,"Variable violating min constraint: 0.0 <= Resistor.T_ref, has value: ");
  modelica_string tmp2;
  modelica_metatype tmpMeta3;
  static int tmp4 = 0;
  if(!tmp4)
  {
    tmp0 = GreaterEq((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[5]] /* Resistor.T_ref PARAM */),0.0);
    if(!tmp0)
    {
      tmp2 = modelica_real_to_modelica_string_format((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[5]] /* Resistor.T_ref PARAM */), (modelica_string) mmc_strings_len1[103]);
      tmpMeta3 = stringAppend(MMC_REFSTRINGLIT(tmp1),tmp2);
      {
        const char* assert_cond = "(Resistor.T_ref >= 0.0)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",5,3,5,64,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta3));
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",5,3,5,64,0};
          omc_assert_warning_withEquationIndexes(info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta3));
        }
      }
      tmp4 = 1;
    }
  }
  threadData->lastEquationSolved = 27;
}

/*
equation index: 28
type: ALGORITHM

  assert(Resistor.T >= 0.0, "Variable violating min constraint: 0.0 <= Resistor.T, has value: " + String(Resistor.T, "g"));
*/
OMC_DISABLE_OPT
static void SimpleSwitchTest_eqFunction_28(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,28};
  modelica_boolean tmp5;
  static const MMC_DEFSTRINGLIT(tmp6,65,"Variable violating min constraint: 0.0 <= Resistor.T, has value: ");
  modelica_string tmp7;
  modelica_metatype tmpMeta8;
  static int tmp9 = 0;
  if(!tmp9)
  {
    tmp5 = GreaterEq((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[3]] /* Resistor.T PARAM */),0.0);
    if(!tmp5)
    {
      tmp7 = modelica_real_to_modelica_string_format((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[3]] /* Resistor.T PARAM */), (modelica_string) mmc_strings_len1[103]);
      tmpMeta8 = stringAppend(MMC_REFSTRINGLIT(tmp6),tmp7);
      {
        const char* assert_cond = "(Resistor.T >= 0.0)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",7,3,8,97,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta8));
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",7,3,8,97,0};
          omc_assert_warning_withEquationIndexes(info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta8));
        }
      }
      tmp9 = 1;
    }
  }
  threadData->lastEquationSolved = 28;
}

/*
equation index: 29
type: ALGORITHM

  assert(Resistor.T_heatPort >= 0.0, "Variable violating min constraint: 0.0 <= Resistor.T_heatPort, has value: " + String(Resistor.T_heatPort, "g"));
*/
OMC_DISABLE_OPT
static void SimpleSwitchTest_eqFunction_29(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,29};
  modelica_boolean tmp10;
  static const MMC_DEFSTRINGLIT(tmp11,74,"Variable violating min constraint: 0.0 <= Resistor.T_heatPort, has value: ");
  modelica_string tmp12;
  modelica_metatype tmpMeta13;
  static int tmp14 = 0;
  if(!tmp14)
  {
    tmp10 = GreaterEq((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[4]] /* Resistor.T_heatPort PARAM */),0.0);
    if(!tmp10)
    {
      tmp12 = modelica_real_to_modelica_string_format((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[4]] /* Resistor.T_heatPort PARAM */), (modelica_string) mmc_strings_len1[103]);
      tmpMeta13 = stringAppend(MMC_REFSTRINGLIT(tmp11),tmp12);
      {
        const char* assert_cond = "(Resistor.T_heatPort >= 0.0)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",14,3,14,54,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta13));
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",14,3,14,54,0};
          omc_assert_warning_withEquationIndexes(info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta13));
        }
      }
      tmp14 = 1;
    }
  }
  threadData->lastEquationSolved = 29;
}
OMC_DISABLE_OPT
void SimpleSwitchTest_updateBoundParameters_0(DATA *data, threadData_t *threadData)
{
  static void (*const eqFunctions[10])(DATA*, threadData_t*) = {
    SimpleSwitchTest_eqFunction_19,
    SimpleSwitchTest_eqFunction_20,
    SimpleSwitchTest_eqFunction_9,
    SimpleSwitchTest_eqFunction_8,
    SimpleSwitchTest_eqFunction_7,
    SimpleSwitchTest_eqFunction_2,
    SimpleSwitchTest_eqFunction_1,
    SimpleSwitchTest_eqFunction_27,
    SimpleSwitchTest_eqFunction_28,
    SimpleSwitchTest_eqFunction_29
  };
  
  for (int id = 0; id < 10; id++) {
    eqFunctions[id](data, threadData);
  }
}
OMC_DISABLE_OPT
int SimpleSwitchTest_updateBoundParameters(DATA *data, threadData_t *threadData)
{
  (data->simulationInfo->booleanParameter[data->simulationInfo->booleanParamsIndex[0]] /* Resistor.useHeatPort PARAM */) = 0 /* false */;
  data->modelData->booleanParameterData[0].time_unvarying = 1;
  SimpleSwitchTest_updateBoundParameters_0(data, threadData);
  return 0;
}

#if defined(__cplusplus)
}
#endif
