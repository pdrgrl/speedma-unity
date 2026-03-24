#include "ShowVariableResistor_FMU.h"

// include fmu header files, typedefs and macros
#include <stdio.h>
#include <string.h>
#include <assert.h>
#include "openmodelica.h"
#include "openmodelica_func.h"
#include "util/omc_error.h"
#include "ShowVariableResistor_functions.h"

#include "simulation/solver/events.h"

// Set values for all variables that define a start value
OMC_DISABLE_OPT
void setDefaultStartValues(ModelInstance *comp) {
  put_real_element(0.0, 0, &comp->fmuData->modelData->realVarsData[0].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[1].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realVarsData[2].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[3].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[4].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[5].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[6].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[7].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[8].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[9].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[10].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[11].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realVarsData[12].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[13].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[14].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[15].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[16].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[17].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[18].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[19].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realVarsData[20].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[21].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[22].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[23].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[24].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[25].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[26].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realVarsData[27].attribute.start);
  put_real_element(1.0, 0, &comp->fmuData->modelData->realParameterData[0].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[1].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[2].attribute.start);
  put_real_element(300.15, 0, &comp->fmuData->modelData->realParameterData[3].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[4].attribute.start);
  put_real_element(1.0, 0, &comp->fmuData->modelData->realParameterData[5].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[6].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[7].attribute.start);
  put_real_element(300.15, 0, &comp->fmuData->modelData->realParameterData[8].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[9].attribute.start);
  put_real_element(1.0, 0, &comp->fmuData->modelData->realParameterData[10].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[11].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[12].attribute.start);
  put_real_element(300.15, 0, &comp->fmuData->modelData->realParameterData[13].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[14].attribute.start);
  put_real_element(1.0, 0, &comp->fmuData->modelData->realParameterData[15].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[16].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[17].attribute.start);
  put_real_element(300.15, 0, &comp->fmuData->modelData->realParameterData[18].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[19].attribute.start);
  put_real_element(1.0, 0, &comp->fmuData->modelData->realParameterData[20].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[21].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[22].attribute.start);
  put_real_element(300.15, 0, &comp->fmuData->modelData->realParameterData[23].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[24].attribute.start);
  put_real_element(1.0, 0, &comp->fmuData->modelData->realParameterData[25].attribute.start);
  put_real_element(1.0, 0, &comp->fmuData->modelData->realParameterData[26].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[27].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[28].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[29].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[30].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[31].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[32].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[33].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[34].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[35].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[36].attribute.start);
  put_real_element(300.15, 0, &comp->fmuData->modelData->realParameterData[37].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[38].attribute.start);
  comp->fmuData->modelData->booleanParameterData[0].attribute.start = 0;
  comp->fmuData->modelData->booleanParameterData[1].attribute.start = 0;
  comp->fmuData->modelData->booleanParameterData[2].attribute.start = 0;
  comp->fmuData->modelData->booleanParameterData[3].attribute.start = 0;
  comp->fmuData->modelData->booleanParameterData[4].attribute.start = 0;
  comp->fmuData->modelData->booleanParameterData[5].attribute.start = 0;
  comp->fmuData->modelData->booleanParameterData[6].attribute.start = 0;
}
// Set values for all variables that define a start value
OMC_DISABLE_OPT
void setStartValues(ModelInstance *comp) {
  put_real_element(comp->fmuData->localData[0]->realVars[0], 0, &comp->fmuData->modelData->realVarsData[0].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[1], 0, &comp->fmuData->modelData->realVarsData[1].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[2], 0, &comp->fmuData->modelData->realVarsData[2].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[3], 0, &comp->fmuData->modelData->realVarsData[3].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[4], 0, &comp->fmuData->modelData->realVarsData[4].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[5], 0, &comp->fmuData->modelData->realVarsData[5].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[6], 0, &comp->fmuData->modelData->realVarsData[6].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[7], 0, &comp->fmuData->modelData->realVarsData[7].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[8], 0, &comp->fmuData->modelData->realVarsData[8].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[9], 0, &comp->fmuData->modelData->realVarsData[9].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[10], 0, &comp->fmuData->modelData->realVarsData[10].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[11], 0, &comp->fmuData->modelData->realVarsData[11].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[12], 0, &comp->fmuData->modelData->realVarsData[12].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[13], 0, &comp->fmuData->modelData->realVarsData[13].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[14], 0, &comp->fmuData->modelData->realVarsData[14].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[15], 0, &comp->fmuData->modelData->realVarsData[15].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[16], 0, &comp->fmuData->modelData->realVarsData[16].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[17], 0, &comp->fmuData->modelData->realVarsData[17].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[18], 0, &comp->fmuData->modelData->realVarsData[18].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[19], 0, &comp->fmuData->modelData->realVarsData[19].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[20], 0, &comp->fmuData->modelData->realVarsData[20].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[21], 0, &comp->fmuData->modelData->realVarsData[21].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[22], 0, &comp->fmuData->modelData->realVarsData[22].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[23], 0, &comp->fmuData->modelData->realVarsData[23].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[24], 0, &comp->fmuData->modelData->realVarsData[24].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[25], 0, &comp->fmuData->modelData->realVarsData[25].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[26], 0, &comp->fmuData->modelData->realVarsData[26].attribute.start);
  put_real_element(comp->fmuData->localData[0]->realVars[27], 0, &comp->fmuData->modelData->realVarsData[27].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[0], 0, &comp->fmuData->modelData->realParameterData[0].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[1], 0, &comp->fmuData->modelData->realParameterData[1].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[2], 0, &comp->fmuData->modelData->realParameterData[2].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[3], 0, &comp->fmuData->modelData->realParameterData[3].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[4], 0, &comp->fmuData->modelData->realParameterData[4].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[5], 0, &comp->fmuData->modelData->realParameterData[5].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[6], 0, &comp->fmuData->modelData->realParameterData[6].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[7], 0, &comp->fmuData->modelData->realParameterData[7].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[8], 0, &comp->fmuData->modelData->realParameterData[8].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[9], 0, &comp->fmuData->modelData->realParameterData[9].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[10], 0, &comp->fmuData->modelData->realParameterData[10].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[11], 0, &comp->fmuData->modelData->realParameterData[11].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[12], 0, &comp->fmuData->modelData->realParameterData[12].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[13], 0, &comp->fmuData->modelData->realParameterData[13].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[14], 0, &comp->fmuData->modelData->realParameterData[14].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[15], 0, &comp->fmuData->modelData->realParameterData[15].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[16], 0, &comp->fmuData->modelData->realParameterData[16].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[17], 0, &comp->fmuData->modelData->realParameterData[17].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[18], 0, &comp->fmuData->modelData->realParameterData[18].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[19], 0, &comp->fmuData->modelData->realParameterData[19].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[20], 0, &comp->fmuData->modelData->realParameterData[20].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[21], 0, &comp->fmuData->modelData->realParameterData[21].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[22], 0, &comp->fmuData->modelData->realParameterData[22].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[23], 0, &comp->fmuData->modelData->realParameterData[23].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[24], 0, &comp->fmuData->modelData->realParameterData[24].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[25], 0, &comp->fmuData->modelData->realParameterData[25].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[26], 0, &comp->fmuData->modelData->realParameterData[26].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[27], 0, &comp->fmuData->modelData->realParameterData[27].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[28], 0, &comp->fmuData->modelData->realParameterData[28].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[29], 0, &comp->fmuData->modelData->realParameterData[29].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[30], 0, &comp->fmuData->modelData->realParameterData[30].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[31], 0, &comp->fmuData->modelData->realParameterData[31].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[32], 0, &comp->fmuData->modelData->realParameterData[32].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[33], 0, &comp->fmuData->modelData->realParameterData[33].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[34], 0, &comp->fmuData->modelData->realParameterData[34].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[35], 0, &comp->fmuData->modelData->realParameterData[35].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[36], 0, &comp->fmuData->modelData->realParameterData[36].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[37], 0, &comp->fmuData->modelData->realParameterData[37].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[38], 0, &comp->fmuData->modelData->realParameterData[38].attribute.start);
  comp->fmuData->modelData->booleanParameterData[0].attribute.start = comp->fmuData->simulationInfo->booleanParameter[0];
  comp->fmuData->modelData->booleanParameterData[1].attribute.start = comp->fmuData->simulationInfo->booleanParameter[1];
  comp->fmuData->modelData->booleanParameterData[2].attribute.start = comp->fmuData->simulationInfo->booleanParameter[2];
  comp->fmuData->modelData->booleanParameterData[3].attribute.start = comp->fmuData->simulationInfo->booleanParameter[3];
  comp->fmuData->modelData->booleanParameterData[4].attribute.start = comp->fmuData->simulationInfo->booleanParameter[4];
  comp->fmuData->modelData->booleanParameterData[5].attribute.start = comp->fmuData->simulationInfo->booleanParameter[5];
  comp->fmuData->modelData->booleanParameterData[6].attribute.start = comp->fmuData->simulationInfo->booleanParameter[6];
}


// implementation of the Model Exchange functions
// Used to set the next time event, if any.
void eventUpdate(ModelInstance* comp, fmi2EventInfo* eventInfo) {
}

static const int realAliasIndexes[33] = {
  -23, 12, -13, 12, -24, 12, -13, 13, 12, 5, -13, 2, 12, 13, 20, -21, 20, -24, -21, 2,
  20, 21, -23, -24, 22, 0, 23, 27, 20, -21, 21, 20, 16
};

fmi2Real getReal(ModelInstance* comp, const fmi2ValueReference vr) {
  if (vr < 28) {
    return comp->fmuData->localData[0]->realVars[vr];
  }
  if (vr < 67) {
    return comp->fmuData->simulationInfo->realParameter[vr-28];
  }
  if (vr < 100) {
    int ix = realAliasIndexes[vr-67];
    return ix>=0 ? getReal(comp, ix) : -getReal(comp, -(ix+1));
  }
  return NAN;
}

fmi2Status setReal(ModelInstance* comp, const fmi2ValueReference vr, const fmi2Real value) {
  // set start value attribute for all variable that has start value, till initialization mode
  if (vr < 28 && (comp->state == model_state_instantiated || comp->state == model_state_initialization_mode)) {
    put_real_element(value, 0, &comp->fmuData->modelData->realVarsData[vr].attribute.start);
  }
  if (vr < 28) {
    comp->fmuData->localData[0]->realVars[vr] = value;
    return fmi2OK;
  }
  if (vr < 67) {
    comp->fmuData->simulationInfo->realParameter[vr-28] = value;
    return fmi2OK;
  }
  if (vr < 100) {
    int ix = realAliasIndexes[vr-67];
    return ix >= 0 ? setReal(comp, ix, value) : setReal(comp, -(ix+1), -value);
  }
  return fmi2Error;
}

fmi2Integer getInteger(ModelInstance* comp, const fmi2ValueReference vr) {
  if (vr < 0) {
    return comp->fmuData->localData[0]->integerVars[vr];
  }
  if (vr < 0) {
    return comp->fmuData->simulationInfo->integerParameter[vr-0];
  }
  return 0;
}

fmi2Status setInteger(ModelInstance* comp, const fmi2ValueReference vr, const fmi2Integer value) {
  // set start value attribute for all variable that has start value, till initialization mode
  if (vr < 0 && (comp->state == model_state_instantiated || comp->state == model_state_initialization_mode)) {
    comp->fmuData->modelData->integerVarsData[vr].attribute.start = value;
  }
  if (vr < 0) {
    comp->fmuData->localData[0]->integerVars[vr] = value;
    return fmi2OK;
  }
  if (vr < 0) {
    comp->fmuData->simulationInfo->integerParameter[vr-0] = value;
    return fmi2OK;
  }
  return fmi2Error;
}
fmi2Boolean getBoolean(ModelInstance* comp, const fmi2ValueReference vr) {
  switch (vr) {
    case 0 : return comp->fmuData->simulationInfo->booleanParameter[0]; break;
    case 1 : return comp->fmuData->simulationInfo->booleanParameter[1]; break;
    case 2 : return comp->fmuData->simulationInfo->booleanParameter[2]; break;
    case 3 : return comp->fmuData->simulationInfo->booleanParameter[3]; break;
    case 4 : return comp->fmuData->simulationInfo->booleanParameter[4]; break;
    case 5 : return comp->fmuData->simulationInfo->booleanParameter[5]; break;
    case 6 : return comp->fmuData->simulationInfo->booleanParameter[6]; break;
    default:
      return fmi2False;
  }
}

fmi2Status setBoolean(ModelInstance* comp, const fmi2ValueReference vr, const fmi2Boolean value) {
  switch (vr) {
    case 0 : comp->fmuData->simulationInfo->booleanParameter[0] = value; break;
    case 1 : comp->fmuData->simulationInfo->booleanParameter[1] = value; break;
    case 2 : comp->fmuData->simulationInfo->booleanParameter[2] = value; break;
    case 3 : comp->fmuData->simulationInfo->booleanParameter[3] = value; break;
    case 4 : comp->fmuData->simulationInfo->booleanParameter[4] = value; break;
    case 5 : comp->fmuData->simulationInfo->booleanParameter[5] = value; break;
    case 6 : comp->fmuData->simulationInfo->booleanParameter[6] = value; break;
    default:
      return fmi2Error;
  }
  return fmi2OK;
}

fmi2String getString(ModelInstance* comp, const fmi2ValueReference vr) {
  switch (vr) {
    default:
      return "";
  }
}

fmi2Status setString(ModelInstance* comp, const fmi2ValueReference vr, fmi2String value) {
  switch (vr) {
    default:
      return fmi2Error;
  }
  return fmi2OK;
}

fmi2Status setExternalFunction(ModelInstance* c, const fmi2ValueReference vr, const void* value){
  switch (vr) {
    default:
      return fmi2Error;
  }
  return fmi2OK;
}

/* function maps input references to a input index used in partialDerivatives */
fmi2ValueReference mapInputReference2InputNumber(const fmi2ValueReference vr) {
    switch (vr) {
      case 27: return 0; break;
      default:
        return -1;
    }
}
/* function maps output references to a input index used in partialDerivatives */
fmi2ValueReference mapOutputReference2OutputNumber(const fmi2ValueReference vr) {
    switch (vr) {
      default:
        return -1;
    }
}
/* function maps output references to an internal output Real derivatives */
fmi2ValueReference mapOutputReference2RealOutputDerivatives(const fmi2ValueReference vr) {
    switch (vr) {
      default:
        return -1;
    }
}
/* function maps initialUnknowns UnknownVars ValueReferences to an internal partial derivatives index */
fmi2ValueReference mapInitialUnknownsdependentIndex(const fmi2ValueReference vr) {
    switch (vr) {
      case 29: return 0; break;
      case 30: return 1; break;
      case 34: return 2; break;
      case 35: return 3; break;
      case 39: return 4; break;
      case 40: return 5; break;
      case 44: return 6; break;
      case 45: return 7; break;
      case 49: return 8; break;
      case 50: return 9; break;
      case 57: return 10; break;
      case 58: return 11; break;
      case 59: return 12; break;
      case 60: return 13; break;
      case 61: return 14; break;
      case 63: return 15; break;
      case 64: return 16; break;
      default:
        return -1;
    }
}
/* function maps initialUnknowns knownVars ValueReferences to an internal partial derivatives index */
fmi2ValueReference mapInitialUnknownsIndependentIndex(const fmi2ValueReference vr) {
    switch (vr) {
      case 31: return 0; break;
      case 36: return 1; break;
      case 41: return 2; break;
      case 46: return 3; break;
      case 51: return 4; break;
      case 53: return 5; break;
      case 54: return 6; break;
      case 55: return 7; break;
      case 56: return 8; break;
      case 62: return 9; break;
      case 65: return 10; break;
      default:
        return -1;
    }
}

