mergeInto(LibraryManager.library, {

  // Called by Unity when a component is selected.
  // Fires window.onUnityComponentSelected(componentId, displayName) on the page.
  NotifyComponentSelected: function (componentIdPtr, displayNamePtr) {
    var componentId  = UTF8ToString(componentIdPtr);
    var displayName  = UTF8ToString(displayNamePtr);
    if (typeof window.onUnityComponentSelected === 'function') {
      window.onUnityComponentSelected(componentId, displayName);
    }
  },

  // Called by Unity when the user clicks empty space (deselects).
  // Fires window.onUnityComponentDeselected() on the page.
  NotifyComponentDeselected: function () {
    if (typeof window.onUnityComponentDeselected === 'function') {
      window.onUnityComponentDeselected();
    }
  }

});
