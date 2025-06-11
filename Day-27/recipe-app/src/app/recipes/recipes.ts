import { Component, OnInit } from '@angular/core';
import { RecipeService } from '../services/recipe.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-recipe',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './recipes.html',
  styleUrl: './recipes.css',
  providers: [RecipeService]
})
export class Recipe implements OnInit {
  recipes: any;

  constructor(private recipeService: RecipeService) {}

  ngOnInit(): void {
    this.recipes = this.recipeService.recipes;
    this.recipeService.fetchAllRecipes();
  }
}
