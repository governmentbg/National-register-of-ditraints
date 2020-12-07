import { PropertyType, RegixSearchCriteriaType } from '../models/enums';

class PropertyHelper {
  getIdentifierTypeCode(propertyType: string, searchByOwner: boolean) {
    let code = "";
    if (propertyType == PropertyType.Vehicle) {
      code = RegixSearchCriteriaType.REGNUMBER;
    } else if (propertyType == PropertyType.Aircraft) {
      code = RegixSearchCriteriaType.MSN;
      if (searchByOwner)
        code = RegixSearchCriteriaType.OWNER;
    } else if(propertyType == PropertyType.Vessel){
      code = RegixSearchCriteriaType.OWNER;
    }
    
    return code;
  }
}

const propertyHelper = new PropertyHelper();
export default propertyHelper;