// Added by GitHub Copilot
// To handle UI for movies by actor

import { apiCall } from '/scripts/common.js';

let selectedActorId = null;

document.addEventListener('DOMContentLoaded', () => {
    loadActors();

    document.getElementById('load-movies-btn').addEventListener('click', loadMoviesForActor);
    document.getElementById('add-movie-form').addEventListener('submit', addMovieToActor);
});

async function loadActors() {
    try {
        const response = await apiCall('/api/v1/actors?page=1&size=100');
        const data = response;
        const actors = data?.data ?? data?.items ?? [];
        const select = document.getElementById('actor-select');
        actors.forEach(actor => {
            const option = document.createElement('option');
            option.value = actor.id;
            option.textContent = actor.name;
            select.appendChild(option);
        });
        select.addEventListener('change', () => {
            selectedActorId = select.value;
        });
    } catch (error) {
        console.error('Error loading actors:', error);
    }
}

async function loadMoviesForActor() {
    if (!selectedActorId) return;

    try {
        const response = await apiCall(`/api/v1/actors/${selectedActorId}/movies`);
        const relationships = response ?? [];
        const movieIds = relationships.map(rel => rel.movieId);
        const movies = [];

        for (const id of movieIds) {
            const movieResponse = await apiCall(`/api/v1/movies/${id}`);
            const movie = JSON.parse(movieResponse);
            movies.push(movie);
        }

        displayMovies(movies);
    } catch (error) {
        console.error('Error loading movies:', error);
    }
}

function displayMovies(movies) {
    const container = document.getElementById('movies-container');
    container.innerHTML = '';

    if (movies.length === 0) {
        container.innerHTML = '<p>No movies found for this actor.</p>';
        return;
    }

    movies.forEach(movie => {
        const div = document.createElement('div');
        div.className = 'movie-card';
        div.innerHTML = `
            <h3>${movie.title} (${movie.year})</h3>
            <p>${movie.description}</p>
            <button onclick="removeMovieFromActor(${movie.id})">Remove from Actor</button>
        `;
        container.appendChild(div);
    });
}

async function addMovieToActor(event) {
    event.preventDefault();
    if (!selectedActorId) {
        alert('Please select an actor first.');
        return;
    }

    const movieId = document.getElementById('movie-id').value;

    try {
        await apiCall('/api/v1/movie-actors', 'POST', { movieId: parseInt(movieId), actorId: parseInt(selectedActorId) });
        loadMoviesForActor();
        document.getElementById('add-movie-form').reset();
    } catch (error) {
        console.error('Error adding movie to actor:', error);
    }
}

window.removeMovieFromActor = async function(movieId) {
    try {
        await apiCall(`/api/v1/movie-actors/${movieId}/${selectedActorId}`, 'DELETE');
        loadMoviesForActor();
    } catch (error) {
        console.error('Error removing movie from actor:', error);
    }
};