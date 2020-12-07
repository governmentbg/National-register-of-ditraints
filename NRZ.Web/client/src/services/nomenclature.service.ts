import { Nomenclature } from '@/models/nomenclature';
import http from '../services/http.service';

class NomenclatureService {
    baseUrl = '/api/nomenclature/';

    
    getPropertyTypes() {
        return http.get<Nomenclature>(`${this.baseUrl}getPropertyTypes`, Nomenclature, true);
    }

    getPropertyTypesForDistraints() {
        return http.get<Nomenclature>(`${this.baseUrl}getPropertyTypesForDistraint`, Nomenclature, true);
    }

    getAuctionOrderTypes() {
        return http.get<Nomenclature>(`${this.baseUrl}AuctionOrderTypes`, Nomenclature, true);
    }

    getUserTypes() {
        return http.get<Nomenclature>(`${this.baseUrl}UserTypes`, Nomenclature, true);
    }

    getUserRegisterTypes() {
        return http.get<Nomenclature>(`${this.baseUrl}UserRegisterTypes`, Nomenclature, true);
    }

    getRegixCompanyStatuses() {
        return http.get<Nomenclature>(`${this.baseUrl}RegixCompanyStatuses`, Nomenclature, true);
    }

    getTimeStampAuthorities() {
        return http.get<Nomenclature>(`${this.baseUrl}TimestampAuthorities`, Nomenclature, true);
    }

    getAssetTypes() {
        return http.get<Nomenclature>(`${this.baseUrl}AssetTypes`, Nomenclature, true);
    }

    getPropertyConstructionType() {
        return http.get<Nomenclature>(`${this.baseUrl}PropertyConstructionType`, Nomenclature, true);
    }

    getPropertyExtras() {
        return http.get<Nomenclature>(`${this.baseUrl}PropertyExtras`, Nomenclature, true);
    }

    getRealEstateTypes() {
        return http.get<Nomenclature>(`${this.baseUrl}RealEstateTypes`, Nomenclature, true);
    }
}

const nomenclatureService = new NomenclatureService();
export default nomenclatureService;