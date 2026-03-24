#include "SimpleSwitchTest_FMU.h"

// include fmu header files, typedefs and macros
#include <stdio.h>
#include <string.h>
#include <assert.h>
#include "openmodelica.h"
#include "openmodelica_func.h"
#include "util/omc_error.h"
#include "SimpleSwitchTest_functions.h"

#include "simulation/solver/events.h"

// Set values for all variables that define a start value
OMC_DISABLE_OPT
void setDefaultStartValues(ModelInstance *comp) {
  put_real_element(0.0, 0, &comp->fmuData->modelData->realVarsData[0].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[1].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realVarsData[2].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realVarsData[3].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realVarsData[4].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[5].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[6].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[7].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realVarsData[8].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realVarsData[9].attribute.start);
  put_real_element(0, 0, &comp->fmuData->modelData->realVarsData[10].attribute.start);
  comp->fmuData->modelData->booleanVarsData[0].attribute.start = 0;
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[0].attribute.start);
  put_real_element(5.0, 0, &comp->fmuData->modelData->realParameterData[1].attribute.start);
  put_real_element(100.0, 0, &comp->fmuData->modelData->realParameterData[2].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[3].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[4].attribute.start);
  put_real_element(300.15, 0, &comp->fmuData->modelData->realParameterData[5].attribute.start);
  put_real_element(0.0, 0, &comp->fmuData->modelData->realParameterData[6].attribute.start);
  comp->fmuData->modelData->booleanParameterData[0].attribute.start = 0;
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
  comp->fmuData->modelData->booleanVarsData[0].attribute.start = comp->fmuData->localData[0]->booleanVars[0];
  put_real_element(comp->fmuData->simulationInfo->realParameter[0], 0, &comp->fmuData->modelData->realParameterData[0].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[1], 0, &comp->fmuData->modelData->realParameterData[1].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[2], 0, &comp->fmuData->modelData->realParameterData[2].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[3], 0, &comp->fmuData->modelData->realParameterData[3].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[4], 0, &comp->fmuData->modelData->realParameterData[4].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[5], 0, &comp->fmuData->modelData->realParameterData[5].attribute.start);
  put_real_element(comp->fmuData->simulationInfo->realParameter[6], 0, &comp->fmuData->modelData->realParameterData[6].attribute.start);
  comp->fmuData->modelData->booleanParameterData[0].attribute.start = comp->fmuData->simulationInfo->booleanParameter[0];
}


// implementation of the Model Exchange functions
// Used to set the next time event, if any.
void eventUpdate(ModelInstance* comp, fmi2EventInfo* eventInfo) {
}

static const int realAliasIndexes[15] = {
  10, -8, 7, 4, -8, 10, 10, -8, 4, 7, 10, 10, 4, 10, 10
};

fmi2Real getReal(ModelInstance* comp, const fmi2ValueReference vr) {
  if (vr < 11) {
    return comp->fmuData->localData[0]->realVars[vr];
  }
  if (vr < 18) {
    return comp->fmuData->simulationInfo->realParameter[vr-11];
  }
  if (vr < 33) {
    int ix = realAliasIndexes[vr-18];
    return ix>=0 ? getReal(comp, ix) : -getReal(comp, -(ix+1));
  }
  return NAN;
}

fmi2Status setReal(ModelInstance* comp, const fmi2ValueReference vr, const fmi2Real value) {
  // set start value attribute for all variable that has start value, till initialization mode
  if (vr < 11 && (comp->state == model_state_instantiated || comp->state == model_state_initialization_mode)) {
    put_real_element(value, 0, &comp->fmuData->modelData->realVarsData[vr].attribute.start);
  }
  if (vr < 11) {
    comp->fmuData->localData[0]->realVars[vr] = value;
    return fmi2OK;
  }
  if (vr < 18) {
    comp->fmuData->simulationInfo->realParameter[vr-11] = value;
    return fmi2OK;
  }
  if (vr < 33) {
    int ix = realAliasIndexes[vr-18];
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
    case 0 : return comp->fmuData->localData[0]->booleanVars[0]; break;
    case 1 : return comp->fmuData->simulationInfo->booleanParameter[0]; break;
    case 2 : return getBoolean(comp, 0); break;
    default:
      return fmi2False;
  }
}

fmi2Status setBoolean(ModelInstance* comp, const fmi2ValueReference vr, const fmi2Boolean value) {
  switch (vr) {
    case 0 : comp->fmuData->localData[0]->booleanVars[0] = value; break;
    case 1 : comp->fmuData->simulationInfo->booleanParameter[0] = value; break;
    case 2 : return setBoolean(comp, 0, value); break;
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
      default:
        return -1;
    }
}
/* function maps output references to a input index used in partialDerivatives */
fmi2ValueReference mapOutputReference2OutputNumber(const fmi2ValueReference vr) {
    switch (vr) {
      case 10: return 0; break;
      default:
        return -1;
    }
}
/* function maps output references to an internal output Real derivatives */
fmi2ValueReference mapOutputReference2RealOutputDerivatives(const fmi2ValueReference vr) {
    switch (vr) {
      case 10: return 2; break;
      default:
        return -1;
    }
}
/* function maps initialUnknowns UnknownVars ValueReferences to an internal partial derivatives index */
fmi2ValueReference mapInitialUnknownsdependentIndex(const fmi2ValueReference vr) {
    switch (vr) {
      case 10: return 0; break;
      case 14: return 1; break;
      case 15: return 2; break;
      default:
        return -1;
    }
}
/* function maps initialUnknowns knownVars ValueReferences to an internal partial derivatives index */
fmi2ValueReference mapInitialUnknownsIndependentIndex(const fmi2ValueReference vr) {
    switch (vr) {
      case 11: return 0; break;
      case 12: return 1; break;
      case 16: return 2; break;
      default:
        return -1;
    }
}

