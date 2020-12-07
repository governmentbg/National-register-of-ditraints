<template>
  <div class="timer-wrapper" :class="{success: time > 600000, error: time <= 600000}">
    <div :id="'timer' + _uid"></div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from "vue-property-decorator";

@Component
export default class TimerComponent extends Vue {
  @Prop({ required: true })
  target!: number;
  private time: number;
  private displayText = "";
  private interval: NodeJS.Timeout | null = null;
  private el!: HTMLElement;

  constructor() {
    super();
    this.time = Date.now();
  }

  startCounter() {
    const that = this;
    console.log("start counter");

    this.interval = setInterval(this.calculateTime, 1000);
  }

  calculateTime() {
    // Find the distance between now and the count down date
    const distance = this.time;

    // Time calculations for days, hours, minutes and seconds
    const days = Math.floor(distance / (1000 * 60 * 60 * 24));
    const hours = Math.floor(
      (distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)
    );
    const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
    const seconds = Math.floor((distance % (1000 * 60)) / 1000);

    // Output the result in an element with id="demo"
    this.displayText =
      days + "д " + hours + "ч " + minutes + "м " + seconds + "с ";

    // If the count down is over, write some text
    if (distance < 0) {
      clearInterval(this.interval!);
      this.displayText = "EXPIRED";
    }
    console.log(this.displayText);

    this.el.innerHTML = this.displayText;
    this.time -= 1000;
  }

  mounted() {
    this.time = this.target;
    this.el = document.getElementById(`timer${this._uid}`)!;
    this.startCounter();
  }
}
</script>

<style lang="scss">
.timer-wrapper {
    // background-color:rgb(58, 236, 117);
    color: white;
    width: 200px;
    padding: 5px;
    font-size: 1.5rem;
    border-radius: 3px;
}

.redTime {
    // background-color:rgb(240, 32, 94);
    color: white;
    width: 200px;
    padding: 5px;
    font-size: 1.5rem;
    border-radius: 3px;
}
</style>