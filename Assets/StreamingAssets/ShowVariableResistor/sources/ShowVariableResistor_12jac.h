/* Jacobians */
static const REAL_ATTRIBUTE dummyREAL_ATTRIBUTE = omc_dummyRealAttribute;

#if defined(__cplusplus)
extern "C" {
#endif

/* Jacobian Variables */
#define ShowVariableResistor_INDEX_JAC_LSJac12 0
int ShowVariableResistor_functionJacLSJac12_column(DATA* data, threadData_t *threadData, JACOBIAN *thisJacobian, JACOBIAN *parentJacobian);
int ShowVariableResistor_initialAnalyticJacobianLSJac12(DATA* data, threadData_t *threadData, JACOBIAN *jacobian);


#define ShowVariableResistor_INDEX_JAC_LSJac13 1
int ShowVariableResistor_functionJacLSJac13_column(DATA* data, threadData_t *threadData, JACOBIAN *thisJacobian, JACOBIAN *parentJacobian);
int ShowVariableResistor_initialAnalyticJacobianLSJac13(DATA* data, threadData_t *threadData, JACOBIAN *jacobian);


#define ShowVariableResistor_INDEX_JAC_LSJac14 2
int ShowVariableResistor_functionJacLSJac14_column(DATA* data, threadData_t *threadData, JACOBIAN *thisJacobian, JACOBIAN *parentJacobian);
int ShowVariableResistor_initialAnalyticJacobianLSJac14(DATA* data, threadData_t *threadData, JACOBIAN *jacobian);


#define ShowVariableResistor_INDEX_JAC_LSJac15 3
int ShowVariableResistor_functionJacLSJac15_column(DATA* data, threadData_t *threadData, JACOBIAN *thisJacobian, JACOBIAN *parentJacobian);
int ShowVariableResistor_initialAnalyticJacobianLSJac15(DATA* data, threadData_t *threadData, JACOBIAN *jacobian);


#define ShowVariableResistor_INDEX_JAC_H 4
int ShowVariableResistor_functionJacH_column(DATA* data, threadData_t *threadData, JACOBIAN *thisJacobian, JACOBIAN *parentJacobian);
int ShowVariableResistor_initialAnalyticJacobianH(DATA* data, threadData_t *threadData, JACOBIAN *jacobian);


#define ShowVariableResistor_INDEX_JAC_F 5
int ShowVariableResistor_functionJacF_column(DATA* data, threadData_t *threadData, JACOBIAN *thisJacobian, JACOBIAN *parentJacobian);
int ShowVariableResistor_initialAnalyticJacobianF(DATA* data, threadData_t *threadData, JACOBIAN *jacobian);


#define ShowVariableResistor_INDEX_JAC_D 6
int ShowVariableResistor_functionJacD_column(DATA* data, threadData_t *threadData, JACOBIAN *thisJacobian, JACOBIAN *parentJacobian);
int ShowVariableResistor_initialAnalyticJacobianD(DATA* data, threadData_t *threadData, JACOBIAN *jacobian);


#define ShowVariableResistor_INDEX_JAC_C 7
int ShowVariableResistor_functionJacC_column(DATA* data, threadData_t *threadData, JACOBIAN *thisJacobian, JACOBIAN *parentJacobian);
int ShowVariableResistor_initialAnalyticJacobianC(DATA* data, threadData_t *threadData, JACOBIAN *jacobian);


#define ShowVariableResistor_INDEX_JAC_B 8
int ShowVariableResistor_functionJacB_column(DATA* data, threadData_t *threadData, JACOBIAN *thisJacobian, JACOBIAN *parentJacobian);
int ShowVariableResistor_initialAnalyticJacobianB(DATA* data, threadData_t *threadData, JACOBIAN *jacobian);


#define ShowVariableResistor_INDEX_JAC_A 9
int ShowVariableResistor_functionJacA_column(DATA* data, threadData_t *threadData, JACOBIAN *thisJacobian, JACOBIAN *parentJacobian);
int ShowVariableResistor_initialAnalyticJacobianA(DATA* data, threadData_t *threadData, JACOBIAN *jacobian);

#if defined(__cplusplus)
}
#endif
