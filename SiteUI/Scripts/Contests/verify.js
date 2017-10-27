
(function () {
    var queryUrl = '/Query/ContestDetail?id=' + $('#contestIdFeed').val();
    $.get(queryUrl, function (data, status) {
        data = JSON.parse(data).data;
        
        if ((data.partMode & 1) !== 0) {
            // Individual mode is supported.
            $('#Participant').append('<option value="(Individual)' + $('#usernameFeed').val() + '"> Individual - ' + $('#usernameFeed').val() + '</option>');
        }
        if ((data.partMode & 2) !== 0) {
            // Teamwork mode is supported.
            // Query user teams.
            queryUrl = '/Query/UserTeams?username=' + $('#usernameFeed').val();
            $.get(queryUrl, function (data, status) {
                data = JSON.parse(data).data;
                for (var i = 0; i < data.length; ++i)
                    $('#Participant').append('<option value="(Teamwork)' + data[i].id + '"> Team - ' + data[i].name + '</option>');
            });
        }
    });
})();
