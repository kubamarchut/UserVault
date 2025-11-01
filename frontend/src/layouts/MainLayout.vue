<template>
  <q-layout view="lHh Lpr lFf">

    <q-header elevated>
      <q-toolbar>
        <q-toolbar-title>
          🏦 User Vault
        </q-toolbar-title>

        <q-btn
          dense
          flat
          round
          :icon="Dark.isActive ? 'light_mode' : 'dark_mode'"
          @click="Dark.toggle()"
          :aria-label="Dark.isActive ? 'Light mode' : 'Dark mode'"
        />
      </q-toolbar>
    </q-header>

    <q-page-container>
      <router-view />
    </q-page-container>

    <q-footer elevated class="bg-grey-8 text-white">
      <div class="q-pa-sm text-center text-caption">
        Made by Jakub Marchut 2025
      </div>
    </q-footer>

  </q-layout>
</template>

<script setup lang="ts">
import { Dark } from 'quasar'
</script>
