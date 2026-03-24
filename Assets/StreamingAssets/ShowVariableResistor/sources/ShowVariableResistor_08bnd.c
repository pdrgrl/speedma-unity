/* update bound parameters and variable attributes (start, nominal, min, max) */
#include "ShowVariableResistor_model.h"
#if defined(__cplusplus)
extern "C" {
#endif

OMC_DISABLE_OPT
int ShowVariableResistor_updateBoundVariableAttributes(DATA *data, threadData_t *threadData)
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

void ShowVariableResistor_updateBoundParameters_0(DATA *data, threadData_t *threadData);

/*
equation index: 78
type: SIMPLE_ASSIGN
VariableResistor.T = VariableResistor.T_ref
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_78(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,78};
  (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[35]] /* VariableResistor.T PARAM */) = (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[37]] /* VariableResistor.T_ref PARAM */);
  threadData->lastEquationSolved = 78;
}

/*
equation index: 79
type: SIMPLE_ASSIGN
VariableResistor.T_heatPort = VariableResistor.T
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_79(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,79};
  (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[36]] /* VariableResistor.T_heatPort PARAM */) = (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[35]] /* VariableResistor.T PARAM */);
  threadData->lastEquationSolved = 79;
}

/*
equation index: 80
type: SIMPLE_ASSIGN
R1.T = R1.T_ref
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_80(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,80};
  (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[1]] /* R1.T PARAM */) = (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[3]] /* R1.T_ref PARAM */);
  threadData->lastEquationSolved = 80;
}

/*
equation index: 81
type: SIMPLE_ASSIGN
R1.T_heatPort = R1.T
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_81(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,81};
  (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[2]] /* R1.T_heatPort PARAM */) = (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[1]] /* R1.T PARAM */);
  threadData->lastEquationSolved = 81;
}

/*
equation index: 82
type: SIMPLE_ASSIGN
R2.T = R2.T_ref
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_82(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,82};
  (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[6]] /* R2.T PARAM */) = (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[8]] /* R2.T_ref PARAM */);
  threadData->lastEquationSolved = 82;
}

/*
equation index: 83
type: SIMPLE_ASSIGN
R2.T_heatPort = R2.T
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_83(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,83};
  (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[7]] /* R2.T_heatPort PARAM */) = (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[6]] /* R2.T PARAM */);
  threadData->lastEquationSolved = 83;
}

/*
equation index: 84
type: SIMPLE_ASSIGN
R3.T = R3.T_ref
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_84(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,84};
  (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[11]] /* R3.T PARAM */) = (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[13]] /* R3.T_ref PARAM */);
  threadData->lastEquationSolved = 84;
}

/*
equation index: 85
type: SIMPLE_ASSIGN
R3.T_heatPort = R3.T
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_85(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,85};
  (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[12]] /* R3.T_heatPort PARAM */) = (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[11]] /* R3.T PARAM */);
  threadData->lastEquationSolved = 85;
}

/*
equation index: 86
type: SIMPLE_ASSIGN
R4.T = R4.T_ref
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_86(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,86};
  (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[16]] /* R4.T PARAM */) = (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[18]] /* R4.T_ref PARAM */);
  threadData->lastEquationSolved = 86;
}

/*
equation index: 87
type: SIMPLE_ASSIGN
R4.T_heatPort = R4.T
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_87(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,87};
  (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[17]] /* R4.T_heatPort PARAM */) = (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[16]] /* R4.T PARAM */);
  threadData->lastEquationSolved = 87;
}

/*
equation index: 88
type: SIMPLE_ASSIGN
R5.T = R5.T_ref
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_88(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,88};
  (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[21]] /* R5.T PARAM */) = (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[23]] /* R5.T_ref PARAM */);
  threadData->lastEquationSolved = 88;
}

/*
equation index: 89
type: SIMPLE_ASSIGN
R5.T_heatPort = R5.T
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_89(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,89};
  (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[22]] /* R5.T_heatPort PARAM */) = (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[21]] /* R5.T PARAM */);
  threadData->lastEquationSolved = 89;
}

/*
equation index: 90
type: SIMPLE_ASSIGN
SineVoltage1.signalSource.startTime = SineVoltage1.startTime
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_90(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,90};
  (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[33]] /* SineVoltage1.signalSource.startTime PARAM */) = (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[34]] /* SineVoltage1.startTime PARAM */);
  threadData->lastEquationSolved = 90;
}

/*
equation index: 91
type: SIMPLE_ASSIGN
SineVoltage1.signalSource.offset = SineVoltage1.offset
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_91(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,91};
  (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[31]] /* SineVoltage1.signalSource.offset PARAM */) = (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[27]] /* SineVoltage1.offset PARAM */);
  threadData->lastEquationSolved = 91;
}

/*
equation index: 93
type: SIMPLE_ASSIGN
SineVoltage1.signalSource.phase = SineVoltage1.phase
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_93(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,93};
  (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[32]] /* SineVoltage1.signalSource.phase PARAM */) = (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[28]] /* SineVoltage1.phase PARAM */);
  threadData->lastEquationSolved = 93;
}

/*
equation index: 94
type: SIMPLE_ASSIGN
SineVoltage1.signalSource.f = SineVoltage1.f
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_94(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,94};
  (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[30]] /* SineVoltage1.signalSource.f PARAM */) = (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[26]] /* SineVoltage1.f PARAM */);
  threadData->lastEquationSolved = 94;
}

/*
equation index: 95
type: SIMPLE_ASSIGN
SineVoltage1.signalSource.amplitude = SineVoltage1.V
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_95(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,95};
  (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[29]] /* SineVoltage1.signalSource.amplitude PARAM */) = (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[25]] /* SineVoltage1.V PARAM */);
  threadData->lastEquationSolved = 95;
}
extern void ShowVariableResistor_eqFunction_39(DATA *data, threadData_t *threadData);

extern void ShowVariableResistor_eqFunction_38(DATA *data, threadData_t *threadData);

extern void ShowVariableResistor_eqFunction_5(DATA *data, threadData_t *threadData);

extern void ShowVariableResistor_eqFunction_4(DATA *data, threadData_t *threadData);

extern void ShowVariableResistor_eqFunction_3(DATA *data, threadData_t *threadData);

extern void ShowVariableResistor_eqFunction_2(DATA *data, threadData_t *threadData);

extern void ShowVariableResistor_eqFunction_1(DATA *data, threadData_t *threadData);


/*
equation index: 109
type: ALGORITHM

  assert(VariableResistor.T_ref >= 0.0, "Variable violating min constraint: 0.0 <= VariableResistor.T_ref, has value: " + String(VariableResistor.T_ref, "g"));
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_109(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,109};
  modelica_boolean tmp0;
  static const MMC_DEFSTRINGLIT(tmp1,77,"Variable violating min constraint: 0.0 <= VariableResistor.T_ref, has value: ");
  modelica_string tmp2;
  modelica_metatype tmpMeta3;
  static int tmp4 = 0;
  if(!tmp4)
  {
    tmp0 = GreaterEq((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[37]] /* VariableResistor.T_ref PARAM */),0.0);
    if(!tmp0)
    {
      tmp2 = modelica_real_to_modelica_string_format((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[37]] /* VariableResistor.T_ref PARAM */), (modelica_string) mmc_strings_len1[103]);
      tmpMeta3 = stringAppend(MMC_REFSTRINGLIT(tmp1),tmp2);
      {
        const char* assert_cond = "(VariableResistor.T_ref >= 0.0)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/VariableResistor.mo",4,3,4,64,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta3));
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/VariableResistor.mo",4,3,4,64,0};
          omc_assert_warning_withEquationIndexes(info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta3));
        }
      }
      tmp4 = 1;
    }
  }
  threadData->lastEquationSolved = 109;
}

/*
equation index: 110
type: ALGORITHM

  assert(VariableResistor.T >= 0.0, "Variable violating min constraint: 0.0 <= VariableResistor.T, has value: " + String(VariableResistor.T, "g"));
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_110(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,110};
  modelica_boolean tmp5;
  static const MMC_DEFSTRINGLIT(tmp6,73,"Variable violating min constraint: 0.0 <= VariableResistor.T, has value: ");
  modelica_string tmp7;
  modelica_metatype tmpMeta8;
  static int tmp9 = 0;
  if(!tmp9)
  {
    tmp5 = GreaterEq((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[35]] /* VariableResistor.T PARAM */),0.0);
    if(!tmp5)
    {
      tmp7 = modelica_real_to_modelica_string_format((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[35]] /* VariableResistor.T PARAM */), (modelica_string) mmc_strings_len1[103]);
      tmpMeta8 = stringAppend(MMC_REFSTRINGLIT(tmp6),tmp7);
      {
        const char* assert_cond = "(VariableResistor.T >= 0.0)";
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
  threadData->lastEquationSolved = 110;
}

/*
equation index: 111
type: ALGORITHM

  assert(VariableResistor.T_heatPort >= 0.0, "Variable violating min constraint: 0.0 <= VariableResistor.T_heatPort, has value: " + String(VariableResistor.T_heatPort, "g"));
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_111(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,111};
  modelica_boolean tmp10;
  static const MMC_DEFSTRINGLIT(tmp11,82,"Variable violating min constraint: 0.0 <= VariableResistor.T_heatPort, has value: ");
  modelica_string tmp12;
  modelica_metatype tmpMeta13;
  static int tmp14 = 0;
  if(!tmp14)
  {
    tmp10 = GreaterEq((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[36]] /* VariableResistor.T_heatPort PARAM */),0.0);
    if(!tmp10)
    {
      tmp12 = modelica_real_to_modelica_string_format((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[36]] /* VariableResistor.T_heatPort PARAM */), (modelica_string) mmc_strings_len1[103]);
      tmpMeta13 = stringAppend(MMC_REFSTRINGLIT(tmp11),tmp12);
      {
        const char* assert_cond = "(VariableResistor.T_heatPort >= 0.0)";
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
  threadData->lastEquationSolved = 111;
}

/*
equation index: 112
type: ALGORITHM

  assert(R1.T_ref >= 0.0, "Variable violating min constraint: 0.0 <= R1.T_ref, has value: " + String(R1.T_ref, "g"));
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_112(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,112};
  modelica_boolean tmp15;
  static const MMC_DEFSTRINGLIT(tmp16,63,"Variable violating min constraint: 0.0 <= R1.T_ref, has value: ");
  modelica_string tmp17;
  modelica_metatype tmpMeta18;
  static int tmp19 = 0;
  if(!tmp19)
  {
    tmp15 = GreaterEq((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[3]] /* R1.T_ref PARAM */),0.0);
    if(!tmp15)
    {
      tmp17 = modelica_real_to_modelica_string_format((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[3]] /* R1.T_ref PARAM */), (modelica_string) mmc_strings_len1[103]);
      tmpMeta18 = stringAppend(MMC_REFSTRINGLIT(tmp16),tmp17);
      {
        const char* assert_cond = "(R1.T_ref >= 0.0)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",5,3,5,64,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta18));
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",5,3,5,64,0};
          omc_assert_warning_withEquationIndexes(info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta18));
        }
      }
      tmp19 = 1;
    }
  }
  threadData->lastEquationSolved = 112;
}

/*
equation index: 113
type: ALGORITHM

  assert(R1.T >= 0.0, "Variable violating min constraint: 0.0 <= R1.T, has value: " + String(R1.T, "g"));
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_113(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,113};
  modelica_boolean tmp20;
  static const MMC_DEFSTRINGLIT(tmp21,59,"Variable violating min constraint: 0.0 <= R1.T, has value: ");
  modelica_string tmp22;
  modelica_metatype tmpMeta23;
  static int tmp24 = 0;
  if(!tmp24)
  {
    tmp20 = GreaterEq((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[1]] /* R1.T PARAM */),0.0);
    if(!tmp20)
    {
      tmp22 = modelica_real_to_modelica_string_format((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[1]] /* R1.T PARAM */), (modelica_string) mmc_strings_len1[103]);
      tmpMeta23 = stringAppend(MMC_REFSTRINGLIT(tmp21),tmp22);
      {
        const char* assert_cond = "(R1.T >= 0.0)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",7,3,8,97,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta23));
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",7,3,8,97,0};
          omc_assert_warning_withEquationIndexes(info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta23));
        }
      }
      tmp24 = 1;
    }
  }
  threadData->lastEquationSolved = 113;
}

/*
equation index: 114
type: ALGORITHM

  assert(R1.T_heatPort >= 0.0, "Variable violating min constraint: 0.0 <= R1.T_heatPort, has value: " + String(R1.T_heatPort, "g"));
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_114(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,114};
  modelica_boolean tmp25;
  static const MMC_DEFSTRINGLIT(tmp26,68,"Variable violating min constraint: 0.0 <= R1.T_heatPort, has value: ");
  modelica_string tmp27;
  modelica_metatype tmpMeta28;
  static int tmp29 = 0;
  if(!tmp29)
  {
    tmp25 = GreaterEq((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[2]] /* R1.T_heatPort PARAM */),0.0);
    if(!tmp25)
    {
      tmp27 = modelica_real_to_modelica_string_format((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[2]] /* R1.T_heatPort PARAM */), (modelica_string) mmc_strings_len1[103]);
      tmpMeta28 = stringAppend(MMC_REFSTRINGLIT(tmp26),tmp27);
      {
        const char* assert_cond = "(R1.T_heatPort >= 0.0)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",14,3,14,54,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta28));
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",14,3,14,54,0};
          omc_assert_warning_withEquationIndexes(info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta28));
        }
      }
      tmp29 = 1;
    }
  }
  threadData->lastEquationSolved = 114;
}

/*
equation index: 115
type: ALGORITHM

  assert(R2.T_ref >= 0.0, "Variable violating min constraint: 0.0 <= R2.T_ref, has value: " + String(R2.T_ref, "g"));
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_115(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,115};
  modelica_boolean tmp30;
  static const MMC_DEFSTRINGLIT(tmp31,63,"Variable violating min constraint: 0.0 <= R2.T_ref, has value: ");
  modelica_string tmp32;
  modelica_metatype tmpMeta33;
  static int tmp34 = 0;
  if(!tmp34)
  {
    tmp30 = GreaterEq((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[8]] /* R2.T_ref PARAM */),0.0);
    if(!tmp30)
    {
      tmp32 = modelica_real_to_modelica_string_format((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[8]] /* R2.T_ref PARAM */), (modelica_string) mmc_strings_len1[103]);
      tmpMeta33 = stringAppend(MMC_REFSTRINGLIT(tmp31),tmp32);
      {
        const char* assert_cond = "(R2.T_ref >= 0.0)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",5,3,5,64,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta33));
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",5,3,5,64,0};
          omc_assert_warning_withEquationIndexes(info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta33));
        }
      }
      tmp34 = 1;
    }
  }
  threadData->lastEquationSolved = 115;
}

/*
equation index: 116
type: ALGORITHM

  assert(R2.T >= 0.0, "Variable violating min constraint: 0.0 <= R2.T, has value: " + String(R2.T, "g"));
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_116(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,116};
  modelica_boolean tmp35;
  static const MMC_DEFSTRINGLIT(tmp36,59,"Variable violating min constraint: 0.0 <= R2.T, has value: ");
  modelica_string tmp37;
  modelica_metatype tmpMeta38;
  static int tmp39 = 0;
  if(!tmp39)
  {
    tmp35 = GreaterEq((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[6]] /* R2.T PARAM */),0.0);
    if(!tmp35)
    {
      tmp37 = modelica_real_to_modelica_string_format((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[6]] /* R2.T PARAM */), (modelica_string) mmc_strings_len1[103]);
      tmpMeta38 = stringAppend(MMC_REFSTRINGLIT(tmp36),tmp37);
      {
        const char* assert_cond = "(R2.T >= 0.0)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",7,3,8,97,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta38));
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",7,3,8,97,0};
          omc_assert_warning_withEquationIndexes(info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta38));
        }
      }
      tmp39 = 1;
    }
  }
  threadData->lastEquationSolved = 116;
}

/*
equation index: 117
type: ALGORITHM

  assert(R2.T_heatPort >= 0.0, "Variable violating min constraint: 0.0 <= R2.T_heatPort, has value: " + String(R2.T_heatPort, "g"));
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_117(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,117};
  modelica_boolean tmp40;
  static const MMC_DEFSTRINGLIT(tmp41,68,"Variable violating min constraint: 0.0 <= R2.T_heatPort, has value: ");
  modelica_string tmp42;
  modelica_metatype tmpMeta43;
  static int tmp44 = 0;
  if(!tmp44)
  {
    tmp40 = GreaterEq((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[7]] /* R2.T_heatPort PARAM */),0.0);
    if(!tmp40)
    {
      tmp42 = modelica_real_to_modelica_string_format((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[7]] /* R2.T_heatPort PARAM */), (modelica_string) mmc_strings_len1[103]);
      tmpMeta43 = stringAppend(MMC_REFSTRINGLIT(tmp41),tmp42);
      {
        const char* assert_cond = "(R2.T_heatPort >= 0.0)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",14,3,14,54,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta43));
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",14,3,14,54,0};
          omc_assert_warning_withEquationIndexes(info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta43));
        }
      }
      tmp44 = 1;
    }
  }
  threadData->lastEquationSolved = 117;
}

/*
equation index: 118
type: ALGORITHM

  assert(R3.T_ref >= 0.0, "Variable violating min constraint: 0.0 <= R3.T_ref, has value: " + String(R3.T_ref, "g"));
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_118(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,118};
  modelica_boolean tmp45;
  static const MMC_DEFSTRINGLIT(tmp46,63,"Variable violating min constraint: 0.0 <= R3.T_ref, has value: ");
  modelica_string tmp47;
  modelica_metatype tmpMeta48;
  static int tmp49 = 0;
  if(!tmp49)
  {
    tmp45 = GreaterEq((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[13]] /* R3.T_ref PARAM */),0.0);
    if(!tmp45)
    {
      tmp47 = modelica_real_to_modelica_string_format((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[13]] /* R3.T_ref PARAM */), (modelica_string) mmc_strings_len1[103]);
      tmpMeta48 = stringAppend(MMC_REFSTRINGLIT(tmp46),tmp47);
      {
        const char* assert_cond = "(R3.T_ref >= 0.0)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",5,3,5,64,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta48));
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",5,3,5,64,0};
          omc_assert_warning_withEquationIndexes(info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta48));
        }
      }
      tmp49 = 1;
    }
  }
  threadData->lastEquationSolved = 118;
}

/*
equation index: 119
type: ALGORITHM

  assert(R3.T >= 0.0, "Variable violating min constraint: 0.0 <= R3.T, has value: " + String(R3.T, "g"));
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_119(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,119};
  modelica_boolean tmp50;
  static const MMC_DEFSTRINGLIT(tmp51,59,"Variable violating min constraint: 0.0 <= R3.T, has value: ");
  modelica_string tmp52;
  modelica_metatype tmpMeta53;
  static int tmp54 = 0;
  if(!tmp54)
  {
    tmp50 = GreaterEq((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[11]] /* R3.T PARAM */),0.0);
    if(!tmp50)
    {
      tmp52 = modelica_real_to_modelica_string_format((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[11]] /* R3.T PARAM */), (modelica_string) mmc_strings_len1[103]);
      tmpMeta53 = stringAppend(MMC_REFSTRINGLIT(tmp51),tmp52);
      {
        const char* assert_cond = "(R3.T >= 0.0)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",7,3,8,97,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta53));
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",7,3,8,97,0};
          omc_assert_warning_withEquationIndexes(info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta53));
        }
      }
      tmp54 = 1;
    }
  }
  threadData->lastEquationSolved = 119;
}

/*
equation index: 120
type: ALGORITHM

  assert(R3.T_heatPort >= 0.0, "Variable violating min constraint: 0.0 <= R3.T_heatPort, has value: " + String(R3.T_heatPort, "g"));
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_120(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,120};
  modelica_boolean tmp55;
  static const MMC_DEFSTRINGLIT(tmp56,68,"Variable violating min constraint: 0.0 <= R3.T_heatPort, has value: ");
  modelica_string tmp57;
  modelica_metatype tmpMeta58;
  static int tmp59 = 0;
  if(!tmp59)
  {
    tmp55 = GreaterEq((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[12]] /* R3.T_heatPort PARAM */),0.0);
    if(!tmp55)
    {
      tmp57 = modelica_real_to_modelica_string_format((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[12]] /* R3.T_heatPort PARAM */), (modelica_string) mmc_strings_len1[103]);
      tmpMeta58 = stringAppend(MMC_REFSTRINGLIT(tmp56),tmp57);
      {
        const char* assert_cond = "(R3.T_heatPort >= 0.0)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",14,3,14,54,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta58));
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",14,3,14,54,0};
          omc_assert_warning_withEquationIndexes(info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta58));
        }
      }
      tmp59 = 1;
    }
  }
  threadData->lastEquationSolved = 120;
}

/*
equation index: 121
type: ALGORITHM

  assert(R4.T_ref >= 0.0, "Variable violating min constraint: 0.0 <= R4.T_ref, has value: " + String(R4.T_ref, "g"));
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_121(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,121};
  modelica_boolean tmp60;
  static const MMC_DEFSTRINGLIT(tmp61,63,"Variable violating min constraint: 0.0 <= R4.T_ref, has value: ");
  modelica_string tmp62;
  modelica_metatype tmpMeta63;
  static int tmp64 = 0;
  if(!tmp64)
  {
    tmp60 = GreaterEq((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[18]] /* R4.T_ref PARAM */),0.0);
    if(!tmp60)
    {
      tmp62 = modelica_real_to_modelica_string_format((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[18]] /* R4.T_ref PARAM */), (modelica_string) mmc_strings_len1[103]);
      tmpMeta63 = stringAppend(MMC_REFSTRINGLIT(tmp61),tmp62);
      {
        const char* assert_cond = "(R4.T_ref >= 0.0)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",5,3,5,64,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta63));
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",5,3,5,64,0};
          omc_assert_warning_withEquationIndexes(info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta63));
        }
      }
      tmp64 = 1;
    }
  }
  threadData->lastEquationSolved = 121;
}

/*
equation index: 122
type: ALGORITHM

  assert(R4.T >= 0.0, "Variable violating min constraint: 0.0 <= R4.T, has value: " + String(R4.T, "g"));
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_122(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,122};
  modelica_boolean tmp65;
  static const MMC_DEFSTRINGLIT(tmp66,59,"Variable violating min constraint: 0.0 <= R4.T, has value: ");
  modelica_string tmp67;
  modelica_metatype tmpMeta68;
  static int tmp69 = 0;
  if(!tmp69)
  {
    tmp65 = GreaterEq((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[16]] /* R4.T PARAM */),0.0);
    if(!tmp65)
    {
      tmp67 = modelica_real_to_modelica_string_format((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[16]] /* R4.T PARAM */), (modelica_string) mmc_strings_len1[103]);
      tmpMeta68 = stringAppend(MMC_REFSTRINGLIT(tmp66),tmp67);
      {
        const char* assert_cond = "(R4.T >= 0.0)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",7,3,8,97,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta68));
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",7,3,8,97,0};
          omc_assert_warning_withEquationIndexes(info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta68));
        }
      }
      tmp69 = 1;
    }
  }
  threadData->lastEquationSolved = 122;
}

/*
equation index: 123
type: ALGORITHM

  assert(R4.T_heatPort >= 0.0, "Variable violating min constraint: 0.0 <= R4.T_heatPort, has value: " + String(R4.T_heatPort, "g"));
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_123(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,123};
  modelica_boolean tmp70;
  static const MMC_DEFSTRINGLIT(tmp71,68,"Variable violating min constraint: 0.0 <= R4.T_heatPort, has value: ");
  modelica_string tmp72;
  modelica_metatype tmpMeta73;
  static int tmp74 = 0;
  if(!tmp74)
  {
    tmp70 = GreaterEq((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[17]] /* R4.T_heatPort PARAM */),0.0);
    if(!tmp70)
    {
      tmp72 = modelica_real_to_modelica_string_format((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[17]] /* R4.T_heatPort PARAM */), (modelica_string) mmc_strings_len1[103]);
      tmpMeta73 = stringAppend(MMC_REFSTRINGLIT(tmp71),tmp72);
      {
        const char* assert_cond = "(R4.T_heatPort >= 0.0)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",14,3,14,54,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta73));
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",14,3,14,54,0};
          omc_assert_warning_withEquationIndexes(info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta73));
        }
      }
      tmp74 = 1;
    }
  }
  threadData->lastEquationSolved = 123;
}

/*
equation index: 124
type: ALGORITHM

  assert(R5.T_ref >= 0.0, "Variable violating min constraint: 0.0 <= R5.T_ref, has value: " + String(R5.T_ref, "g"));
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_124(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,124};
  modelica_boolean tmp75;
  static const MMC_DEFSTRINGLIT(tmp76,63,"Variable violating min constraint: 0.0 <= R5.T_ref, has value: ");
  modelica_string tmp77;
  modelica_metatype tmpMeta78;
  static int tmp79 = 0;
  if(!tmp79)
  {
    tmp75 = GreaterEq((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[23]] /* R5.T_ref PARAM */),0.0);
    if(!tmp75)
    {
      tmp77 = modelica_real_to_modelica_string_format((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[23]] /* R5.T_ref PARAM */), (modelica_string) mmc_strings_len1[103]);
      tmpMeta78 = stringAppend(MMC_REFSTRINGLIT(tmp76),tmp77);
      {
        const char* assert_cond = "(R5.T_ref >= 0.0)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",5,3,5,64,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta78));
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",5,3,5,64,0};
          omc_assert_warning_withEquationIndexes(info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta78));
        }
      }
      tmp79 = 1;
    }
  }
  threadData->lastEquationSolved = 124;
}

/*
equation index: 125
type: ALGORITHM

  assert(R5.T >= 0.0, "Variable violating min constraint: 0.0 <= R5.T, has value: " + String(R5.T, "g"));
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_125(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,125};
  modelica_boolean tmp80;
  static const MMC_DEFSTRINGLIT(tmp81,59,"Variable violating min constraint: 0.0 <= R5.T, has value: ");
  modelica_string tmp82;
  modelica_metatype tmpMeta83;
  static int tmp84 = 0;
  if(!tmp84)
  {
    tmp80 = GreaterEq((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[21]] /* R5.T PARAM */),0.0);
    if(!tmp80)
    {
      tmp82 = modelica_real_to_modelica_string_format((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[21]] /* R5.T PARAM */), (modelica_string) mmc_strings_len1[103]);
      tmpMeta83 = stringAppend(MMC_REFSTRINGLIT(tmp81),tmp82);
      {
        const char* assert_cond = "(R5.T >= 0.0)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",7,3,8,97,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta83));
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",7,3,8,97,0};
          omc_assert_warning_withEquationIndexes(info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta83));
        }
      }
      tmp84 = 1;
    }
  }
  threadData->lastEquationSolved = 125;
}

/*
equation index: 126
type: ALGORITHM

  assert(R5.T_heatPort >= 0.0, "Variable violating min constraint: 0.0 <= R5.T_heatPort, has value: " + String(R5.T_heatPort, "g"));
*/
OMC_DISABLE_OPT
static void ShowVariableResistor_eqFunction_126(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,126};
  modelica_boolean tmp85;
  static const MMC_DEFSTRINGLIT(tmp86,68,"Variable violating min constraint: 0.0 <= R5.T_heatPort, has value: ");
  modelica_string tmp87;
  modelica_metatype tmpMeta88;
  static int tmp89 = 0;
  if(!tmp89)
  {
    tmp85 = GreaterEq((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[22]] /* R5.T_heatPort PARAM */),0.0);
    if(!tmp85)
    {
      tmp87 = modelica_real_to_modelica_string_format((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[22]] /* R5.T_heatPort PARAM */), (modelica_string) mmc_strings_len1[103]);
      tmpMeta88 = stringAppend(MMC_REFSTRINGLIT(tmp86),tmp87);
      {
        const char* assert_cond = "(R5.T_heatPort >= 0.0)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",14,3,14,54,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta88));
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Interfaces/ConditionalHeatPort.mo",14,3,14,54,0};
          omc_assert_warning_withEquationIndexes(info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(tmpMeta88));
        }
      }
      tmp89 = 1;
    }
  }
  threadData->lastEquationSolved = 126;
}
OMC_DISABLE_OPT
void ShowVariableResistor_updateBoundParameters_0(DATA *data, threadData_t *threadData)
{
  static void (*const eqFunctions[42])(DATA*, threadData_t*) = {
    ShowVariableResistor_eqFunction_78,
    ShowVariableResistor_eqFunction_79,
    ShowVariableResistor_eqFunction_80,
    ShowVariableResistor_eqFunction_81,
    ShowVariableResistor_eqFunction_82,
    ShowVariableResistor_eqFunction_83,
    ShowVariableResistor_eqFunction_84,
    ShowVariableResistor_eqFunction_85,
    ShowVariableResistor_eqFunction_86,
    ShowVariableResistor_eqFunction_87,
    ShowVariableResistor_eqFunction_88,
    ShowVariableResistor_eqFunction_89,
    ShowVariableResistor_eqFunction_90,
    ShowVariableResistor_eqFunction_91,
    ShowVariableResistor_eqFunction_93,
    ShowVariableResistor_eqFunction_94,
    ShowVariableResistor_eqFunction_95,
    ShowVariableResistor_eqFunction_39,
    ShowVariableResistor_eqFunction_38,
    ShowVariableResistor_eqFunction_5,
    ShowVariableResistor_eqFunction_4,
    ShowVariableResistor_eqFunction_3,
    ShowVariableResistor_eqFunction_2,
    ShowVariableResistor_eqFunction_1,
    ShowVariableResistor_eqFunction_109,
    ShowVariableResistor_eqFunction_110,
    ShowVariableResistor_eqFunction_111,
    ShowVariableResistor_eqFunction_112,
    ShowVariableResistor_eqFunction_113,
    ShowVariableResistor_eqFunction_114,
    ShowVariableResistor_eqFunction_115,
    ShowVariableResistor_eqFunction_116,
    ShowVariableResistor_eqFunction_117,
    ShowVariableResistor_eqFunction_118,
    ShowVariableResistor_eqFunction_119,
    ShowVariableResistor_eqFunction_120,
    ShowVariableResistor_eqFunction_121,
    ShowVariableResistor_eqFunction_122,
    ShowVariableResistor_eqFunction_123,
    ShowVariableResistor_eqFunction_124,
    ShowVariableResistor_eqFunction_125,
    ShowVariableResistor_eqFunction_126
  };
  
  for (int id = 0; id < 42; id++) {
    eqFunctions[id](data, threadData);
  }
}
OMC_DISABLE_OPT
int ShowVariableResistor_updateBoundParameters(DATA *data, threadData_t *threadData)
{
  (data->simulationInfo->booleanParameter[data->simulationInfo->booleanParamsIndex[0]] /* R1.useHeatPort PARAM */) = 0 /* false */;
  data->modelData->booleanParameterData[0].time_unvarying = 1;
  (data->simulationInfo->booleanParameter[data->simulationInfo->booleanParamsIndex[1]] /* R2.useHeatPort PARAM */) = 0 /* false */;
  data->modelData->booleanParameterData[1].time_unvarying = 1;
  (data->simulationInfo->booleanParameter[data->simulationInfo->booleanParamsIndex[2]] /* R3.useHeatPort PARAM */) = 0 /* false */;
  data->modelData->booleanParameterData[2].time_unvarying = 1;
  (data->simulationInfo->booleanParameter[data->simulationInfo->booleanParamsIndex[3]] /* R4.useHeatPort PARAM */) = 0 /* false */;
  data->modelData->booleanParameterData[3].time_unvarying = 1;
  (data->simulationInfo->booleanParameter[data->simulationInfo->booleanParamsIndex[4]] /* R5.useHeatPort PARAM */) = 0 /* false */;
  data->modelData->booleanParameterData[4].time_unvarying = 1;
  (data->simulationInfo->booleanParameter[data->simulationInfo->booleanParamsIndex[5]] /* SineVoltage1.signalSource.continuous PARAM */) = 0 /* false */;
  data->modelData->booleanParameterData[5].time_unvarying = 1;
  (data->simulationInfo->booleanParameter[data->simulationInfo->booleanParamsIndex[6]] /* VariableResistor.useHeatPort PARAM */) = 0 /* false */;
  data->modelData->booleanParameterData[6].time_unvarying = 1;
  ShowVariableResistor_updateBoundParameters_0(data, threadData);
  return 0;
}

#if defined(__cplusplus)
}
#endif
