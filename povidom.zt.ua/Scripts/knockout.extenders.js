(function (ko) {

    ko.extenders.required = function (target) {
        target.isValid = ko.computed(function () {
            return target() && target().trim().length > 0;
        });
        return target;
    };

    ko.extenders.phone = function (target) {
        var result = ko.computed({
            read: target,
            write: function (newValue) {
                var
                    currentValue = target(),
                    valueToWrite
                ;
                if (typeof newValue == typeof undefined) {
                    valueToWrite = undefined;
                }
                else if (isNaN(newValue)) {
                    valueToWrite = currentValue;
                } else {
                    valueToWrite = newValue;
                }

                if (valueToWrite != currentValue) {
                    target(valueToWrite);
                } else {
                    if (newValue != currentValue) {
                        target.notifySubscribers(valueToWrite);
                    }
                }

            }
        }).extend({ notify: 'always' });

        result.isValid = ko.computed(function () {
            return !!result();
        });

        result(target());

        return result;
    };

})(window.ko);

