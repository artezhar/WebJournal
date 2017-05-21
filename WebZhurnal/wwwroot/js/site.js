function utChange(element) {
    if ($(element).val() === 'Teacher') {
        $('#sGroupId').val('');
        $('#sGroupId').attr('disabled', 'disabled');
        $('#sSubject').removeAttr('disabled', '');
        return;
    }
    
    if ($(element).val() === 'Student') {
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