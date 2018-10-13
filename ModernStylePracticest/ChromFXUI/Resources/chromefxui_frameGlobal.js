﻿(function () {
	const ATTR_NAME = "cfx-ui-command";

	if (!window["raiseCustomEvent"]) {
		window["raiseCustomEvent"] = function (eventName, customDetail) {
			window.dispatchEvent(new CustomEvent(eventName, { detail: customDetail }));
		};
	}

	
	window.addEventListener("click", (e) => {
		var targetEl = e.srcElement;

		while (targetEl && !targetEl.hasAttribute(ATTR_NAME)) {
			targetEl = targetEl.parentElement;
		}

		if (targetEl) {
			var cmd = targetEl.getAttribute(ATTR_NAME);

			if (cmd && ChromeFXUI) {
			    ChromeFXUI.hostWindow[cmd].apply(ChromeFXUI, [e]);
			}
		}
	});

	raiseCustomEvent("hostactivestate", { state: 1, stateName: "activated" });
})();

