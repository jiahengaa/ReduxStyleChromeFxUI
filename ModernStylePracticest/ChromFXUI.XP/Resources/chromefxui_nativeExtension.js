var ChromeFXUI = ChromeFXUI || {};

(function (chormefxui) {
    chormefxui.__defineGetter__("version", function () {
		native function GetVersion();
		return GetVersion();
	});

})(ChromeFXUI);