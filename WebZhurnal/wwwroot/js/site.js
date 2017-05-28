function utChange(element) {
    if ($(element).val() === 'Teacher' || $(element).val() == 0) {
        $('#sGroupId').val('');
        $('#sGroupId').attr('disabled', 'disabled');
        $('#sSubject').removeAttr('disabled', '');
        return;
    }
    
    if ($(element).val() === 'Student' || $(element).val() == 1) {
        $('#sGroupId').removeAttr('disabled', '');
        $('#sSubject').attr('disabled', 'disabled');
        $('#sSubject').val('');
        return;
    }

    $('#sGroupId').attr('disabled', 'disabled');
    $('#sSubject').attr('disabled', 'disabled');
    $('#sGroupId').val('');
    $('#sSubject').val('');
}