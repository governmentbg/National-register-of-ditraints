import {Route} from 'vue-router/types';

class Helper {
    castIdRouteParams(route: Route) {
        const id = Number.parseInt(route.params.id, 10);
        if (Number.isNaN(id)) {
            return 0;
        }
        return { id }
    }

    disableInput() {
        const toDisable: HTMLCollectionOf<Element> = document.getElementsByClassName("disabled")!;
        for(let i = 0; i < toDisable.length; i++){
            toDisable[i].querySelector("input")!.setAttribute("disabled", "disabled");
        }
    }
}

const helper = new Helper();
export default helper;