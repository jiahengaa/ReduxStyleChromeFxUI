var ChromeFXUI = ChromeFXUI || {};

(function (chromefxui) {
    chromefxui.__defineGetter__("version", function () {
		native function GetVersion();
		return GetVersion();
	});

})(ChromeFXUI);