function togglePark(modelId) {
    $form = $(this);
    options = {
        url: "/Vehicles/TogglePark/" + modelId
    }

    $.ajax(options).done(function (data) {
        var cbIx = data.lastIndexOf("<td");
        if (cbIx > 0) {
            $('#timeParkedTableData' + modelId).replaceWith(data.substring(0, cbIx));
            $($form.attr("id")).replaceWith(data.substring(cbIx));
        }
        else
            window.location = data;
    });

    return false;
}

