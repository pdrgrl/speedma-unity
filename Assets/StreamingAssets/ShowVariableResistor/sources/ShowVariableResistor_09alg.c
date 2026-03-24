/* Algebraic */
#include "ShowVariableResistor_model.h"

#ifdef __cplusplus
extern "C" {
#endif

/* forwarded equations */
extern void ShowVariableResistor_eqFunction_40(DATA* data, threadData_t *threadData);
extern void ShowVariableResistor_eqFunction_41(DATA* data, threadData_t *threadData);
extern void ShowVariableResistor_eqFunction_52(DATA* data, threadData_t *threadData);
extern void ShowVariableResistor_eqFunction_53(DATA* data, threadData_t *threadData);
extern void ShowVariableResistor_eqFunction_54(DATA* data, threadData_t *threadData);
extern void ShowVariableResistor_eqFunction_55(DATA* data, threadData_t *threadData);
extern void ShowVariableResistor_eqFunction_66(DATA* data, threadData_t *threadData);
extern void ShowVariableResistor_eqFunction_67(DATA* data, threadData_t *threadData);
extern void ShowVariableResistor_eqFunction_68(DATA* data, threadData_t *threadData);
extern void ShowVariableResistor_eqFunction_69(DATA* data, threadData_t *threadData);
extern void ShowVariableResistor_eqFunction_70(DATA* data, threadData_t *threadData);
extern void ShowVariableResistor_eqFunction_71(DATA* data, threadData_t *threadData);
extern void ShowVariableResistor_eqFunction_72(DATA* data, threadData_t *threadData);
extern void ShowVariableResistor_eqFunction_73(DATA* data, threadData_t *threadData);
extern void ShowVariableResistor_eqFunction_74(DATA* data, threadData_t *threadData);
extern void ShowVariableResistor_eqFunction_75(DATA* data, threadData_t *threadData);
extern void ShowVariableResistor_eqFunction_76(DATA* data, threadData_t *threadData);
extern void ShowVariableResistor_eqFunction_77(DATA* data, threadData_t *threadData);

static void functionAlg_system0(DATA *data, threadData_t *threadData)
{
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
    ShowVariableResistor_eqFunction_72,
    ShowVariableResistor_eqFunction_73,
    ShowVariableResistor_eqFunction_74,
    ShowVariableResistor_eqFunction_75,
    ShowVariableResistor_eqFunction_76,
    ShowVariableResistor_eqFunction_77
  };
  
  for (int id = 0; id < 18; id++) {
    eqFunctions[id](data, threadData);
  }
}
/* for continuous time variables */
int ShowVariableResistor_functionAlgebraics(DATA *data, threadData_t *threadData)
{

#if !defined(OMC_MINIMAL_RUNTIME)
  if (measure_time_flag) rt_tick(SIM_TIMER_ALGEBRAICS);
#endif
  data->simulationInfo->callStatistics.functionAlgebraics++;

  ShowVariableResistor_function_savePreSynchronous(data, threadData);
  
  functionAlg_system0(data, threadData);

#if !defined(OMC_MINIMAL_RUNTIME)
  if (measure_time_flag) rt_accumulate(SIM_TIMER_ALGEBRAICS);
#endif

  return 0;
}

#ifdef __cplusplus
}
#endif
