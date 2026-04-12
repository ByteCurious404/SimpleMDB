// Added by GitHub Copilot
// To handle UI interactions for movie-actor relationships

import { apiCall } from '/scripts/common.js';

document.addEventListener('DOMContentLoaded', () => {
    loadRelationships();

    const form = document.getElementById('add-relationship-form');
    form.addEventListener('submit', addRelationship);
});

async function loadRelationships() {
    try {
        const response = await apiCall('/api/v1/movie-actors');
        const data = response;
        const relationships = data?.data ?? data?.items ?? [];
        displayRelationships(relationships);
    } catch (error) {
        console.error('Error loading relationships:', error);
    }
}

function displayRelationships(relationships) {
    const container = document.getElementById('relationships-container');
    container.innerHTML = '';

    if (relationships.length === 0) {
        container.innerHTML = '<p>No relationships found.</p>';
        return;
    }

    relationships.forEach(rel => {
        const div = document.createElement('div');
        div.className = 'relationship-card';
        div.innerHTML = `
            <h3>Movie ID: ${rel.movieId} - Actor ID: ${rel.actorId}</h3>
            <button onclick="deleteRelationship(${rel.movieId}, ${rel.actorId})">Delete</button>
        `;
        container.appendChild(div);
    });
}

async function addRelationship(event) {
    event.preventDefault();

    const movieId = document.getElementById('movie-id').value;
    const actorId = document.getElementById('actor-id').value;

    try {
        await apiCall('/api/v1/movie-actors', 'POST', { movieId: parseInt(movieId), actorId: parseInt(actorId) });
        loadRelationships();
        document.getElementById('add-relationship-form').reset();
    } catch (error) {
        console.error('Error adding relationship:', error);
    }
}

window.deleteRelationship = async function(movieId, actorId) {
    try {
        await apiCall(`/api/v1/movie-actors/${movieId}/${actorId}`, 'DELETE');
        loadRelationships();
    } catch (error) {
        console.error('Error deleting relationship:', error);
    }
};