import { Component } from '@angular/core';
import { Recipe } from './recipes/recipes';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [Recipe],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'recipe-app';
}
