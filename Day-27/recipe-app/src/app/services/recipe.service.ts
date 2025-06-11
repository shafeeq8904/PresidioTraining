import { Injectable, Signal, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RecipeModel } from '../models/recipe.model';
import { catchError, of } from 'rxjs';

@Injectable()
export class RecipeService {
  private http = inject(HttpClient);
  private recipesSignal = signal<RecipeModel[]>([]);

  get recipes(): Signal<RecipeModel[]> {
    return this.recipesSignal.asReadonly();
  }

  fetchAllRecipes() {
    this.http.get<{ recipes: RecipeModel[] }>('https://dummyjson.com/recipes')
      .pipe(catchError(() => of({ recipes: [] })))
      .subscribe(response => {
        this.recipesSignal.set(response.recipes);
      });
  }
}
