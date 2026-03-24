/* Algebraic */
#include "SimpleSwitchTest_model.h"

#ifdef __cplusplus
extern "C" {
#endif

/* forwarded equations */
extern void SimpleSwitchTest_eqFunction_12(DATA* data, threadData_t *threadData);
extern void SimpleSwitchTest_eqFunction_13(DATA* data, threadData_t *threadData);
extern void SimpleSwitchTest_eqFunction_14(DATA* data, threadData_t *threadData);
extern void SimpleSwitchTest_eqFunction_15(DATA* data, threadData_t *threadData);
extern void SimpleSwitchTest_eqFunction_16(DATA* data, threadData_t *threadData);
extern void SimpleSwitchTest_eqFunction_17(DATA* data, threadData_t *threadData);
extern void SimpleSwitchTest_eqFunction_18(DATA* data, threadData_t *threadData);

static void functionAlg_system0(DATA *data, threadData_t *threadData)
{
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
}
/* for continuous time variables */
int SimpleSwitchTest_functionAlgebraics(DATA *data, threadData_t *threadData)
{

#if !defined(OMC_MINIMAL_RUNTIME)
  if (measure_time_flag) rt_tick(SIM_TIMER_ALGEBRAICS);
#endif
  data->simulationInfo->callStatistics.functionAlgebraics++;

  SimpleSwitchTest_function_savePreSynchronous(data, threadData);
  
  functionAlg_system0(data, threadData);

#if !defined(OMC_MINIMAL_RUNTIME)
  if (measure_time_flag) rt_accumulate(SIM_TIMER_ALGEBRAICS);
#endif

  return 0;
}

#ifdef __cplusplus
}
#endif
