// Added by GitHub Copilot
// To handle UI for actors in movie

import { apiCall } from '/scripts/common.js';

let selectedMovieId = null;

document.addEventListener('DOMContentLoaded', () => {
    loadMovies();

    document.getElementById('load-actors-btn').addEventListener('click', loadActorsForMovie);
    document.getElementById('add-actor-form').addEventListener('submit', addActorToMovie);
});

async function loadMovies() {
    try {
        const response = await apiCall('/api/v1/movies?page=1&size=100');
        const data = response;
        const movies = data?.data ?? data?.items ?? [];
        const select = document.getElementById('movie-select');
        movies.forEach(movie => {
            const option = document.createElement('option');
            option.value = movie.id;
            option.textContent = movie.title;
            select.appendChild(option);
        });
        select.addEventListener('change', () => {
            selectedMovieId = select.value;
        });
    } catch (error) {
        console.error('Error loading movies:', error);
    }
}

async function loadActorsForMovie() {
    if (!selectedMovieId) return;

    try {
        const response = await apiCall(`/api/v1/movies/${selectedMovieId}/actors`);
        const relationships = response ?? [];
        const actorIds = relationships.map(rel => rel.actorId);
        const actors = [];

        for (const id of actorIds) {
            const actorResponse = await apiCall(`/api/v1/actors/${id}`);
            const actor = JSON.parse(actorResponse);
            actors.push(actor);
        }

        displayActors(actors);
    } catch (error) {
        console.error('Error loading actors:', error);
    }
}

function displayActors(actors) {
    const container = document.getElementById('actors-container');
    container.innerHTML = '';

    if (actors.length === 0) {
        container.innerHTML = '<p>No actors found for this movie.</p>';
        return;
    }

    actors.forEach(actor => {
        const div = document.createElement('div');
        div.className = 'actor-card';
        div.innerHTML = `
            <h3>${actor.name} (${actor.birthYear})</h3>
            <p>${actor.biography}</p>
            <button onclick="removeActorFromMovie(${actor.id})">Remove from Movie</button>
        `;
        container.appendChild(div);
    });
}

async function addActorToMovie(event) {
    event.preventDefault();
    if (!selectedMovieId) {
        alert('Please select a movie first.');
        return;
    }

    const actorId = document.getElementById('actor-id').value;

    try {
        await apiCall('/api/v1/movie-actors', 'POST', { movieId: parseInt(selectedMovieId), actorId: parseInt(actorId) });
        loadActorsForMovie();
        document.getElementById('add-actor-form').reset();
    } catch (error) {
        console.error('Error adding actor to movie:', error);
    }
}

window.removeActorFromMovie = async function(actorId) {
    try {
        await apiCall(`/api/v1/movie-actors/${selectedMovieId}/${actorId}`, 'DELETE');
        loadActorsForMovie();
    } catch (error) {
        console.error('Error removing actor from movie:', error);
    }
};