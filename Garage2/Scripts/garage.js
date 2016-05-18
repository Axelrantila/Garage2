﻿function togglePark(modelId) {
    options = {
        url: "/Vehicles/TogglePark/" + modelId
    }

    $.ajax(options).done(function (data) {
        var cbIx = data.indexOf("<tr");
        if (cbIx >= 0) {
            $('#vehicleTableRow' + modelId).replaceWith(data);
            //var tpSubData = data.substring(0, cbIx),
            //    pSubData = data.substring(cbIx);
            //$('#timeParkedTableData' + modelId).replaceWith(tpSubData);
            //$('#parkToggleForm' + modelId).replaceWith(pSubData);
            //$($form.attr("id")).replaceWith(pSubData);
        }
        else
            window.location = data;
    });

    return false;
}

