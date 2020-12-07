import { extend } from 'vee-validate';
import * as rules from 'vee-validate/dist/rules';
import '../language/validatorMessages';

// Add all validation rules
for (const [rule, validation] of Object.entries(rules)) {
  extend(rule, {
    ...validation
  });
}


extend('compareFields', {
  params: ['target'],
  validate(value: any, {target}: any) {
    return value === target;
  }
});

const pattern = new RegExp(/^(?=.*\d)(?=.*[a-zа-я])(?=.*[A-ZА-Я])(?=.*[a-zA-Zа-яА-Я])(?=.*[`~!@#$%^&*()_\-+=|\\/<>.,'":;[\]{}€]).{6,}$/g);

extend('passwordPattern', {
  validate(value: any) {
    return value.match(pattern)
  }
})

extend('isPropertyChecked', {
  params: ['checkedProperties'],
  validate(value: any, {propertyItems}: any) {
    // todo: enable this rule
    if (propertyItems && propertyItems[0].id != null) {
      return true;
    } else {
      return true;
    }
  }
})