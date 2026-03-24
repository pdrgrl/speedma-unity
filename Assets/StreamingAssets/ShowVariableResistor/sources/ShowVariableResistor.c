/* Main Simulation File */

#if defined(__cplusplus)
extern "C" {
#endif

#include "ShowVariableResistor_model.h"
#include "simulation/solver/events.h"
#include "util/real_array.h"



/* dummy VARINFO and FILEINFO */
const VAR_INFO dummyVAR_INFO = omc_dummyVarInfo;

int ShowVariableResistor_input_function(DATA *data, threadData_t *threadData)
{
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[27]] /* u variable */) = data->simulationInfo->inputVars[0];
  
  return 0;
}

int ShowVariableResistor_input_function_init(DATA *data, threadData_t *threadData)
{
  assertStreamPrint(threadData, data->modelData->realVarsData[27].dimension.numberOfDimensions == 0, "Handling of array variables not yet implemetned.");
  data->simulationInfo->inputVars[0] = real_get(data->modelData->realVarsData[27].attribute.start, 0);
  
  return 0;
}

int ShowVariableResistor_input_function_updateStartValues(DATA *data, threadData_t *threadData)
{
  assertStreamPrint(threadData, data->modelData->realVarsData[27].dimension.numberOfDimensions == 0, "Handling of array variables not yet implemetned.");
  put_real_element(data->simulationInfo->inputVars[0], 0, &data->modelData->realVarsData[27].attribute.start);
  
  return 0;
}

int ShowVariableResistor_inputNames(DATA *data, char ** names){
  names[0] = (char *) data->modelData->realVarsData[27].info.name;
  
  return 0;
}

int ShowVariableResistor_data_function(DATA *data, threadData_t *threadData)
{
  return 0;
}

int ShowVariableResistor_dataReconciliationInputNames(DATA *data, char ** names){
  
  return 0;
}

int ShowVariableResistor_dataReconciliationUnmeasuredVariables(DATA *data, char ** names)
{
  
  return 0;
}

int ShowVariableResistor_output_function(DATA *data, threadData_t *threadData)
{
  
  return 0;
}

int ShowVariableResistor_setc_function(DATA *data, threadData_t *threadData)
{
  
  return 0;
}

int ShowVariableResistor_setb_function(DATA *data, threadData_t *threadData)
{
  
  return 0;
}


/*
equation index: 40
type: SIMPLE_ASSIGN
VariableResistor.R_actual = u * (1.0 + VariableResistor.alpha * (VariableResistor.T - VariableResistor.T_ref))
*/
void ShowVariableResistor_eqFunction_40(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,40};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[25]] /* VariableResistor.R_actual variable */) = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[27]] /* u variable */)) * (1.0 + ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[38]] /* VariableResistor.alpha PARAM */)) * ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[35]] /* VariableResistor.T PARAM */) - (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[37]] /* VariableResistor.T_ref PARAM */)));
  threadData->lastEquationSolved = 40;
}

/*
equation index: 41
type: SIMPLE_ASSIGN
SineVoltage1.v = SineVoltage1.signalSource.offset + (if time < SineVoltage1.signalSource.startTime then 0.0 else SineVoltage1.signalSource.amplitude * sin(6.283185307179586 * SineVoltage1.signalSource.f * (time - SineVoltage1.signalSource.startTime) + SineVoltage1.signalSource.phase))
*/
void ShowVariableResistor_eqFunction_41(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,41};
  modelica_boolean tmp0;
  modelica_real tmp1;
  modelica_real tmp2;
  tmp1 = 1.0;
  tmp2 = fabs((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[33]] /* SineVoltage1.signalSource.startTime PARAM */));
  relationhysteresis(data, &tmp0, data->localData[0]->timeValue, (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[33]] /* SineVoltage1.signalSource.startTime PARAM */), tmp1, tmp2, 0, Less, LessZC);
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[23]] /* SineVoltage1.v variable */) = (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[31]] /* SineVoltage1.signalSource.offset PARAM */) + (tmp0?0.0:((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[29]] /* SineVoltage1.signalSource.amplitude PARAM */)) * (sin((6.283185307179586) * (((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[30]] /* SineVoltage1.signalSource.f PARAM */)) * (data->localData[0]->timeValue - (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[33]] /* SineVoltage1.signalSource.startTime PARAM */))) + (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[32]] /* SineVoltage1.signalSource.phase PARAM */))));
  threadData->lastEquationSolved = 41;
}

/*
equation index: 52
type: LINEAR

<var>R3.i</var>
<row>

</row>
<matrix>
</matrix>
*/
OMC_DISABLE_OPT
void ShowVariableResistor_eqFunction_52(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,52};
  /* Linear equation system */
  int retValue;
  double aux_x[1] = { (data->localData[1]->realVars[data->simulationInfo->realVarsIndex[12]] /* R3.i variable */) };
  infoStreamPrint(OMC_LOG_DT, 0, "Solving linear system 52 (STRICT TEARING SET if tearing enabled) at time = %18.10e", data->localData[0]->timeValue);

  retValue = solve_linear_system(data, threadData, 2, &aux_x[0]);

  /* check if solution process was successful */
  if (retValue > 0){
    const int indexes[2] = {1,52};
    throwStreamPrintWithEquationIndexes(threadData, omc_dummyFileInfo, indexes, "Solving linear system 52 failed at time=%.15g.\nFor more information please use -lv LOG_LS.", data->localData[0]->timeValue);
  }
  /* write solution */
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[12]] /* R3.i variable */) = aux_x[0];

  threadData->lastEquationSolved = 52;
}

/*
equation index: 53
type: SIMPLE_ASSIGN
R1.LossPower = R1.v * R3.i
*/
void ShowVariableResistor_eqFunction_53(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,53};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[3]] /* R1.LossPower variable */) = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[6]] /* R1.v variable */)) * ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[12]] /* R3.i variable */));
  threadData->lastEquationSolved = 53;
}

/*
equation index: 54
type: SIMPLE_ASSIGN
R2.LossPower = R2.v * R3.i
*/
void ShowVariableResistor_eqFunction_54(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,54};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[7]] /* R2.LossPower variable */) = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[9]] /* R2.v variable */)) * ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[12]] /* R3.i variable */));
  threadData->lastEquationSolved = 54;
}

/*
equation index: 55
type: SIMPLE_ASSIGN
R3.LossPower = R3.v * R3.i
*/
void ShowVariableResistor_eqFunction_55(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,55};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[10]] /* R3.LossPower variable */) = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[13]] /* R3.v variable */)) * ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[12]] /* R3.i variable */));
  threadData->lastEquationSolved = 55;
}

/*
equation index: 66
type: LINEAR

<var>R5.i</var>
<row>

</row>
<matrix>
</matrix>
*/
OMC_DISABLE_OPT
void ShowVariableResistor_eqFunction_66(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,66};
  /* Linear equation system */
  int retValue;
  double aux_x[1] = { (data->localData[1]->realVars[data->simulationInfo->realVarsIndex[20]] /* R5.i variable */) };
  infoStreamPrint(OMC_LOG_DT, 0, "Solving linear system 66 (STRICT TEARING SET if tearing enabled) at time = %18.10e", data->localData[0]->timeValue);

  retValue = solve_linear_system(data, threadData, 3, &aux_x[0]);

  /* check if solution process was successful */
  if (retValue > 0){
    const int indexes[2] = {1,66};
    throwStreamPrintWithEquationIndexes(threadData, omc_dummyFileInfo, indexes, "Solving linear system 66 failed at time=%.15g.\nFor more information please use -lv LOG_LS.", data->localData[0]->timeValue);
  }
  /* write solution */
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[20]] /* R5.i variable */) = aux_x[0];

  threadData->lastEquationSolved = 66;
}

/*
equation index: 67
type: SIMPLE_ASSIGN
R4.LossPower = R4.v * R5.i
*/
void ShowVariableResistor_eqFunction_67(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,67};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[14]] /* R4.LossPower variable */) = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[17]] /* R4.v variable */)) * ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[20]] /* R5.i variable */));
  threadData->lastEquationSolved = 67;
}

/*
equation index: 68
type: SIMPLE_ASSIGN
VariableResistor.LossPower = VariableResistor.v * R5.i
*/
void ShowVariableResistor_eqFunction_68(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,68};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[24]] /* VariableResistor.LossPower variable */) = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[26]] /* VariableResistor.v variable */)) * ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[20]] /* R5.i variable */));
  threadData->lastEquationSolved = 68;
}

/*
equation index: 69
type: SIMPLE_ASSIGN
SineVoltage1.i = R5.i + R3.i
*/
void ShowVariableResistor_eqFunction_69(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,69};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[22]] /* SineVoltage1.i variable */) = (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[20]] /* R5.i variable */) + (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[12]] /* R3.i variable */);
  threadData->lastEquationSolved = 69;
}

/*
equation index: 70
type: SIMPLE_ASSIGN
Ground2.p.i = SineVoltage1.i
*/
void ShowVariableResistor_eqFunction_70(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,70};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[1]] /* Ground2.p.i variable */) = (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[22]] /* SineVoltage1.i variable */);
  threadData->lastEquationSolved = 70;
}

/*
equation index: 71
type: SIMPLE_ASSIGN
R5.LossPower = R5.v * R5.i
*/
void ShowVariableResistor_eqFunction_71(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,71};
  (data->localData[0]->realVars[data->simulationInfo->realVarsIndex[18]] /* R5.LossPower variable */) = ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[21]] /* R5.v variable */)) * ((data->localData[0]->realVars[data->simulationInfo->realVarsIndex[20]] /* R5.i variable */));
  threadData->lastEquationSolved = 71;
}

/*
equation index: 77
type: ALGORITHM

  assert(1.0 + R5.alpha * (R5.T - R5.T_ref) >= 2.220446049250313e-16, "Temperature outside scope of model!");
*/
void ShowVariableResistor_eqFunction_77(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,77};
  modelica_boolean tmp3;
  static const MMC_DEFSTRINGLIT(tmp4,35,"Temperature outside scope of model!");
  static int tmp5 = 0;
  {
    tmp3 = GreaterEq(1.0 + ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[24]] /* R5.alpha PARAM */)) * ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[21]] /* R5.T PARAM */) - (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[23]] /* R5.T_ref PARAM */)),2.220446049250313e-16);
    if(!tmp3)
    {
      {
        const char* assert_cond = "(1.0 + R5.alpha * (R5.T - R5.T_ref) >= 2.220446049250313e-16)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",15,3,16,43,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(MMC_REFSTRINGLIT(tmp4)));
          data->simulationInfo->needToReThrow = 1;
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",15,3,16,43,0};
          omc_assert_withEquationIndexes(threadData, info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(MMC_REFSTRINGLIT(tmp4)));
        }
      }
    }
  }
  threadData->lastEquationSolved = 77;
}

/*
equation index: 76
type: ALGORITHM

  assert(1.0 + R4.alpha * (R4.T - R4.T_ref) >= 2.220446049250313e-16, "Temperature outside scope of model!");
*/
void ShowVariableResistor_eqFunction_76(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,76};
  modelica_boolean tmp6;
  static const MMC_DEFSTRINGLIT(tmp7,35,"Temperature outside scope of model!");
  static int tmp8 = 0;
  {
    tmp6 = GreaterEq(1.0 + ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[19]] /* R4.alpha PARAM */)) * ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[16]] /* R4.T PARAM */) - (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[18]] /* R4.T_ref PARAM */)),2.220446049250313e-16);
    if(!tmp6)
    {
      {
        const char* assert_cond = "(1.0 + R4.alpha * (R4.T - R4.T_ref) >= 2.220446049250313e-16)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",15,3,16,43,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(MMC_REFSTRINGLIT(tmp7)));
          data->simulationInfo->needToReThrow = 1;
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",15,3,16,43,0};
          omc_assert_withEquationIndexes(threadData, info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(MMC_REFSTRINGLIT(tmp7)));
        }
      }
    }
  }
  threadData->lastEquationSolved = 76;
}

/*
equation index: 75
type: ALGORITHM

  assert(1.0 + R3.alpha * (R3.T - R3.T_ref) >= 2.220446049250313e-16, "Temperature outside scope of model!");
*/
void ShowVariableResistor_eqFunction_75(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,75};
  modelica_boolean tmp9;
  static const MMC_DEFSTRINGLIT(tmp10,35,"Temperature outside scope of model!");
  static int tmp11 = 0;
  {
    tmp9 = GreaterEq(1.0 + ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[14]] /* R3.alpha PARAM */)) * ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[11]] /* R3.T PARAM */) - (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[13]] /* R3.T_ref PARAM */)),2.220446049250313e-16);
    if(!tmp9)
    {
      {
        const char* assert_cond = "(1.0 + R3.alpha * (R3.T - R3.T_ref) >= 2.220446049250313e-16)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",15,3,16,43,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(MMC_REFSTRINGLIT(tmp10)));
          data->simulationInfo->needToReThrow = 1;
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",15,3,16,43,0};
          omc_assert_withEquationIndexes(threadData, info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(MMC_REFSTRINGLIT(tmp10)));
        }
      }
    }
  }
  threadData->lastEquationSolved = 75;
}

/*
equation index: 74
type: ALGORITHM

  assert(1.0 + R2.alpha * (R2.T - R2.T_ref) >= 2.220446049250313e-16, "Temperature outside scope of model!");
*/
void ShowVariableResistor_eqFunction_74(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,74};
  modelica_boolean tmp12;
  static const MMC_DEFSTRINGLIT(tmp13,35,"Temperature outside scope of model!");
  static int tmp14 = 0;
  {
    tmp12 = GreaterEq(1.0 + ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[9]] /* R2.alpha PARAM */)) * ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[6]] /* R2.T PARAM */) - (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[8]] /* R2.T_ref PARAM */)),2.220446049250313e-16);
    if(!tmp12)
    {
      {
        const char* assert_cond = "(1.0 + R2.alpha * (R2.T - R2.T_ref) >= 2.220446049250313e-16)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",15,3,16,43,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(MMC_REFSTRINGLIT(tmp13)));
          data->simulationInfo->needToReThrow = 1;
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",15,3,16,43,0};
          omc_assert_withEquationIndexes(threadData, info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(MMC_REFSTRINGLIT(tmp13)));
        }
      }
    }
  }
  threadData->lastEquationSolved = 74;
}

/*
equation index: 73
type: ALGORITHM

  assert(1.0 + R1.alpha * (R1.T - R1.T_ref) >= 2.220446049250313e-16, "Temperature outside scope of model!");
*/
void ShowVariableResistor_eqFunction_73(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,73};
  modelica_boolean tmp15;
  static const MMC_DEFSTRINGLIT(tmp16,35,"Temperature outside scope of model!");
  static int tmp17 = 0;
  {
    tmp15 = GreaterEq(1.0 + ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[4]] /* R1.alpha PARAM */)) * ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[1]] /* R1.T PARAM */) - (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[3]] /* R1.T_ref PARAM */)),2.220446049250313e-16);
    if(!tmp15)
    {
      {
        const char* assert_cond = "(1.0 + R1.alpha * (R1.T - R1.T_ref) >= 2.220446049250313e-16)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",15,3,16,43,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(MMC_REFSTRINGLIT(tmp16)));
          data->simulationInfo->needToReThrow = 1;
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/Resistor.mo",15,3,16,43,0};
          omc_assert_withEquationIndexes(threadData, info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(MMC_REFSTRINGLIT(tmp16)));
        }
      }
    }
  }
  threadData->lastEquationSolved = 73;
}

/*
equation index: 72
type: ALGORITHM

  assert(1.0 + VariableResistor.alpha * (VariableResistor.T - VariableResistor.T_ref) >= 2.220446049250313e-16, "Temperature outside scope of model!");
*/
void ShowVariableResistor_eqFunction_72(DATA *data, threadData_t *threadData)
{
  const int equationIndexes[2] = {1,72};
  modelica_boolean tmp18;
  static const MMC_DEFSTRINGLIT(tmp19,35,"Temperature outside scope of model!");
  static int tmp20 = 0;
  {
    tmp18 = GreaterEq(1.0 + ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[38]] /* VariableResistor.alpha PARAM */)) * ((data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[35]] /* VariableResistor.T PARAM */) - (data->simulationInfo->realParameter[data->simulationInfo->realParamsIndex[37]] /* VariableResistor.T_ref PARAM */)),2.220446049250313e-16);
    if(!tmp18)
    {
      {
        const char* assert_cond = "(1.0 + VariableResistor.alpha * (VariableResistor.T - VariableResistor.T_ref) >= 2.220446049250313e-16)";
        if (data->simulationInfo->noThrowAsserts) {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/VariableResistor.mo",20,3,21,43,0};
          infoStreamPrintWithEquationIndexes(OMC_LOG_ASSERT, info, 0, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(MMC_REFSTRINGLIT(tmp19)));
          data->simulationInfo->needToReThrow = 1;
        } else {
          FILE_INFO info = {"C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om/Electrical/Analog/Basic/VariableResistor.mo",20,3,21,43,0};
          omc_assert_withEquationIndexes(threadData, info, equationIndexes, "The following assertion has been violated %sat time %f\n(%s) --> \"%s\"", initial() ? "during initialization " : "", data->localData[0]->timeValue, assert_cond, MMC_STRINGDATA(MMC_REFSTRINGLIT(tmp19)));
        }
      }
    }
  }
  threadData->lastEquationSolved = 72;
}

OMC_DISABLE_OPT
int ShowVariableResistor_functionDAE(DATA *data, threadData_t *threadData)
{
  int equationIndexes[1] = {0};
#if !defined(OMC_MINIMAL_RUNTIME)
  if (measure_time_flag) rt_tick(SIM_TIMER_DAE);
#endif

  data->simulationInfo->needToIterate = 0;
  data->simulationInfo->discreteCall = 1;
  ShowVariableResistor_functionLocalKnownVars(data, threadData);
  static void (*const eqFunctions[18])(DATA*, threadData_t*) = {
    ShowVariableResistor_eqFunction_40,
    ShowVariableResistor_eqFunction_41,
    ShowVariableResistor_eqFunction_52,
    ShowVariableResistor_eqFunction_53,
    ShowVariableResistor_eqFunction_54,
    ShowVariableResistor_eqFunction_55,
    ShowVariableResistor_eqFunction_66,
    ShowVariableResistor_eqFunction_67,
    ShowVariableResistor_eqFunction_68,
    ShowVariableResistor_eqFunction_69,
    ShowVariableResistor_eqFunction_70,
    ShowVariableResistor_eqFunction_71,
    ShowVariableResistor_eqFunction_77,
    ShowVariableResistor_eqFunction_76,
    ShowVariableResistor_eqFunction_75,
    ShowVariableResistor_eqFunction_74,
    ShowVariableResistor_eqFunction_73,
    ShowVariableResistor_eqFunction_72
  };
  
  for (int id = 0; id < 18; id++) {
    eqFunctions[id](data, threadData);
  }
  data->simulationInfo->discreteCall = 0;
  
#if !defined(OMC_MINIMAL_RUNTIME)
  if (measure_time_flag) rt_accumulate(SIM_TIMER_DAE);
#endif
  return 0;
}


int ShowVariableResistor_functionLocalKnownVars(DATA *data, threadData_t *threadData)
{
  
  return 0;
}


int ShowVariableResistor_functionODE(DATA *data, threadData_t *threadData)
{
#if !defined(OMC_MINIMAL_RUNTIME)
  if (measure_time_flag) rt_tick(SIM_TIMER_FUNCTION_ODE);
#endif

  
  data->simulationInfo->callStatistics.functionODE++;
  
  ShowVariableResistor_functionLocalKnownVars(data, threadData);
  /* no ODE systems */

#if !defined(OMC_MINIMAL_RUNTIME)
  if (measure_time_flag) rt_accumulate(SIM_TIMER_FUNCTION_ODE);
#endif

  return 0;
}

/* forward the main in the simulation runtime */
extern int _main_SimulationRuntime(int argc, char **argv, DATA *data, threadData_t *threadData);
extern int _main_OptimizationRuntime(int argc, char **argv, DATA *data, threadData_t *threadData);

#include "ShowVariableResistor_12jac.h"
#include "ShowVariableResistor_13opt.h"

struct OpenModelicaGeneratedFunctionCallbacks ShowVariableResistor_callback = {
  NULL,    /* performSimulation */
  NULL,    /* performQSSSimulation */
  NULL,    /* updateContinuousSystem */
  ShowVariableResistor_callExternalObjectDestructors,    /* callExternalObjectDestructors */
  NULL,    /* initialNonLinearSystem */
  ShowVariableResistor_initialLinearSystem,    /* initialLinearSystem */
  NULL,    /* initialMixedSystem */
  #if !defined(OMC_NO_STATESELECTION)
  ShowVariableResistor_initializeStateSets,
  #else
  NULL,
  #endif    /* initializeStateSets */
  ShowVariableResistor_initializeDAEmodeData,
  ShowVariableResistor_functionODE,
  ShowVariableResistor_functionAlgebraics,
  ShowVariableResistor_functionDAE,
  ShowVariableResistor_functionLocalKnownVars,
  ShowVariableResistor_input_function,
  ShowVariableResistor_input_function_init,
  ShowVariableResistor_input_function_updateStartValues,
  ShowVariableResistor_data_function,
  ShowVariableResistor_output_function,
  ShowVariableResistor_setc_function,
  ShowVariableResistor_setb_function,
  ShowVariableResistor_function_storeDelayed,
  ShowVariableResistor_function_storeSpatialDistribution,
  ShowVariableResistor_function_initSpatialDistribution,
  ShowVariableResistor_updateBoundVariableAttributes,
  ShowVariableResistor_functionInitialEquations,
  GLOBAL_EQUIDISTANT_HOMOTOPY,
  NULL,
  ShowVariableResistor_functionRemovedInitialEquations,
  ShowVariableResistor_updateBoundParameters,
  ShowVariableResistor_checkForAsserts,
  ShowVariableResistor_function_ZeroCrossingsEquations,
  ShowVariableResistor_function_ZeroCrossings,
  ShowVariableResistor_function_updateRelations,
  ShowVariableResistor_zeroCrossingDescription,
  ShowVariableResistor_relationDescription,
  ShowVariableResistor_function_initSample,
  ShowVariableResistor_INDEX_JAC_A,
  ShowVariableResistor_INDEX_JAC_B,
  ShowVariableResistor_INDEX_JAC_C,
  ShowVariableResistor_INDEX_JAC_D,
  ShowVariableResistor_INDEX_JAC_F,
  ShowVariableResistor_INDEX_JAC_H,
  ShowVariableResistor_initialAnalyticJacobianA,
  ShowVariableResistor_initialAnalyticJacobianB,
  ShowVariableResistor_initialAnalyticJacobianC,
  ShowVariableResistor_initialAnalyticJacobianD,
  ShowVariableResistor_initialAnalyticJacobianF,
  ShowVariableResistor_initialAnalyticJacobianH,
  ShowVariableResistor_functionJacA_column,
  ShowVariableResistor_functionJacB_column,
  ShowVariableResistor_functionJacC_column,
  ShowVariableResistor_functionJacD_column,
  ShowVariableResistor_functionJacF_column,
  ShowVariableResistor_functionJacH_column,
  ShowVariableResistor_linear_model_frame,
  ShowVariableResistor_linear_model_datarecovery_frame,
  ShowVariableResistor_mayer,
  ShowVariableResistor_lagrange,
  ShowVariableResistor_getInputVarIndicesInOptimization,
  ShowVariableResistor_pickUpBoundsForInputsInOptimization,
  ShowVariableResistor_setInputData,
  ShowVariableResistor_getTimeGrid,
  ShowVariableResistor_symbolicInlineSystem,
  ShowVariableResistor_function_initSynchronous,
  ShowVariableResistor_function_updateSynchronous,
  ShowVariableResistor_function_equationsSynchronous,
  ShowVariableResistor_inputNames,
  ShowVariableResistor_dataReconciliationInputNames,
  ShowVariableResistor_dataReconciliationUnmeasuredVariables,
  ShowVariableResistor_read_simulation_info,
  ShowVariableResistor_read_input_fmu,
  NULL,
  NULL,
  -1,
  NULL,
  NULL,
  -1

};

#define _OMC_LIT_RESOURCE_0_name_data "CircuitoTeste"
#define _OMC_LIT_RESOURCE_0_dir_data "."
static const MMC_DEFSTRINGLIT(_OMC_LIT_RESOURCE_0_name,13,_OMC_LIT_RESOURCE_0_name_data);
static const MMC_DEFSTRINGLIT(_OMC_LIT_RESOURCE_0_dir,1,_OMC_LIT_RESOURCE_0_dir_data);

#define _OMC_LIT_RESOURCE_1_name_data "Complex"
#define _OMC_LIT_RESOURCE_1_dir_data "C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Complex 4.1.0+maint.om"
static const MMC_DEFSTRINGLIT(_OMC_LIT_RESOURCE_1_name,7,_OMC_LIT_RESOURCE_1_name_data);
static const MMC_DEFSTRINGLIT(_OMC_LIT_RESOURCE_1_dir,77,_OMC_LIT_RESOURCE_1_dir_data);

#define _OMC_LIT_RESOURCE_2_name_data "Modelica"
#define _OMC_LIT_RESOURCE_2_dir_data "C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/Modelica 4.1.0+maint.om"
static const MMC_DEFSTRINGLIT(_OMC_LIT_RESOURCE_2_name,8,_OMC_LIT_RESOURCE_2_name_data);
static const MMC_DEFSTRINGLIT(_OMC_LIT_RESOURCE_2_dir,78,_OMC_LIT_RESOURCE_2_dir_data);

#define _OMC_LIT_RESOURCE_3_name_data "ModelicaServices"
#define _OMC_LIT_RESOURCE_3_dir_data "C:/Users/pedro/AppData/Roaming/.openmodelica/libraries/ModelicaServices 4.1.0+maint.om"
static const MMC_DEFSTRINGLIT(_OMC_LIT_RESOURCE_3_name,16,_OMC_LIT_RESOURCE_3_name_data);
static const MMC_DEFSTRINGLIT(_OMC_LIT_RESOURCE_3_dir,86,_OMC_LIT_RESOURCE_3_dir_data);

#define _OMC_LIT_RESOURCE_4_name_data "ShowVariableResistor"
#define _OMC_LIT_RESOURCE_4_dir_data "Y:/Downloads"
static const MMC_DEFSTRINGLIT(_OMC_LIT_RESOURCE_4_name,20,_OMC_LIT_RESOURCE_4_name_data);
static const MMC_DEFSTRINGLIT(_OMC_LIT_RESOURCE_4_dir,12,_OMC_LIT_RESOURCE_4_dir_data);

static const MMC_DEFSTRUCTLIT(_OMC_LIT_RESOURCES,10,MMC_ARRAY_TAG) {MMC_REFSTRINGLIT(_OMC_LIT_RESOURCE_0_name), MMC_REFSTRINGLIT(_OMC_LIT_RESOURCE_0_dir), MMC_REFSTRINGLIT(_OMC_LIT_RESOURCE_1_name), MMC_REFSTRINGLIT(_OMC_LIT_RESOURCE_1_dir), MMC_REFSTRINGLIT(_OMC_LIT_RESOURCE_2_name), MMC_REFSTRINGLIT(_OMC_LIT_RESOURCE_2_dir), MMC_REFSTRINGLIT(_OMC_LIT_RESOURCE_3_name), MMC_REFSTRINGLIT(_OMC_LIT_RESOURCE_3_dir), MMC_REFSTRINGLIT(_OMC_LIT_RESOURCE_4_name), MMC_REFSTRINGLIT(_OMC_LIT_RESOURCE_4_dir)}};
void ShowVariableResistor_setupDataStruc(DATA *data, threadData_t *threadData)
{
  assertStreamPrint(threadData,0!=data, "Error while initialize Data");
  threadData->localRoots[LOCAL_ROOT_SIMULATION_DATA] = data;
  data->callback = &ShowVariableResistor_callback;
  OpenModelica_updateUriMapping(threadData, MMC_REFSTRUCTLIT(_OMC_LIT_RESOURCES));
  data->modelData->modelName = "ShowVariableResistor";
  data->modelData->modelFilePrefix = "ShowVariableResistor";
  data->modelData->modelFileName = "ShowVariableResistor.mo";
  data->modelData->resultFileName = NULL;
  data->modelData->modelDir = "Y:/Downloads";
  data->modelData->modelGUID = "{35c2bc86-17a9-47f2-8908-32d385838396}";
  data->modelData->initXMLData = NULL;
  data->modelData->modelDataXml.infoXMLData = NULL;
  GC_asprintf(&data->modelData->modelDataXml.fileName, "%s/ShowVariableResistor_info.json", data->modelData->resourcesDir);
  data->modelData->runTestsuite = 0;
  data->modelData->nStatesArray = 0;
  data->modelData->nDiscreteReal = 0;
  data->modelData->nVariablesRealArray = 28;
  data->modelData->nVariablesIntegerArray = 0;
  data->modelData->nVariablesBooleanArray = 0;
  data->modelData->nVariablesStringArray = 0;
  data->modelData->nParametersRealArray = 39;
  data->modelData->nParametersIntegerArray = 0;
  data->modelData->nParametersBooleanArray = 7;
  data->modelData->nParametersStringArray = 0;
  data->modelData->nParametersReal = 39;
  data->modelData->nParametersInteger = 0;
  data->modelData->nParametersBoolean = 7;
  data->modelData->nParametersString = 0;
  data->modelData->nAliasRealArray = 33;
  data->modelData->nAliasIntegerArray = 0;
  data->modelData->nAliasBooleanArray = 0;
  data->modelData->nAliasStringArray = 0;
  data->modelData->nInputVars = 1;
  data->modelData->nOutputVars = 0;
  data->modelData->nZeroCrossings = 1;
  data->modelData->nSamples = 0;
  data->modelData->nRelations = 1;
  data->modelData->nMathEvents = 0;
  data->modelData->nExtObjs = 0;
  data->modelData->modelDataXml.modelInfoXmlLength = 0;
  data->modelData->modelDataXml.nFunctions = 0;
  data->modelData->modelDataXml.nProfileBlocks = 0;
  data->modelData->modelDataXml.nEquations = 127;
  data->modelData->nMixedSystems = 0;
  data->modelData->nLinearSystems = 4;
  data->modelData->nNonLinearSystems = 0;
  data->modelData->nStateSets = 0;
  data->modelData->nJacobians = 10;
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

