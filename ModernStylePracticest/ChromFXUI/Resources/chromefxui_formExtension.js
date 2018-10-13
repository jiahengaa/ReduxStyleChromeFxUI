var ChromeFXUI = ChromeFXUI || {};

(function (chormefxui) {
    chormefxui.hostWindow = {
		minimize: function () {
			native function Minimize();
			return Minimize();
		},
		maximize: function () {
			native function Maximize();
			return Maximize();
		},
		restore: function () {
			native function Restore();
			return Restore();
		},
		close: function () {
			native function Close();
			return Close();
		}
	};

	chormefxui.hostWindow.__defineGetter__("currentState", function () {
		native function GetWinState();
		return GetWinState();
	});

	chormefxui.hostWindow.__defineGetter__("isActivated", function () {
		native function GetWinActivated();
		return GetWinActivated();
	});

})(ChromeFXUI);
