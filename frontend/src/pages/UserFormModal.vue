<template>
  <q-dialog
    :model-value="modelValue"
    @update:model-value="emit('update:modelValue', $event)"
    :maximized="$q.screen.lt.sm"
  >
    <q-card :style="{
        minWidth: $q.screen.lt.sm ? '100%' : '500px',
        width: $q.screen.lt.sm ? '100%' : 'auto',
        maxHeight: '90vh',
        overflowY: 'auto'
      }">
      <q-card-section>
        <div class="text-h6">{{ isEdit ? 'Edit User' : 'Add User' }}</div>
      </q-card-section>

      <q-card-section class="q-gutter-md q-pt-md">
        <q-input
          v-model="form.firstname"
          label="First Name"
          outlined
          dense
          hide-bottom-space
          :rules="[
            val => !!val || 'First name is required',
            val => val.length <= 50 || 'Max 50 characters',
            val => /^[a-zA-Z\u00C0-\u017F\s]*$/.test(val) || 'Only letters and spaces are allowed'
          ]"
        />
        <q-input
          v-model="form.lastname"
          label="Last Name"
          outlined
          dense
          hide-bottom-space
          :rules="[
            val => !!val || 'Last name is required',
            val => val.length <= 150 || 'Max 150 characters',
            val => /^[a-zA-Z\u00C0-\u017F\s-]*$/.test(val) || 'Only letters, spaces, and hyphens'
          ]"
        />
        <q-input
          v-model="dateOfBirthString"
          label="Date of Birth"
          type="date"
          outlined
          dense
          hide-bottom-space
          :rules="[
            val => !!val || 'Date of birth is required',
            val => {
              const parts = val.split('-').map(Number);
              const selectedDate = new Date(parts[0], parts[1] - 1, parts[2]);
              const today = new Date();
              today.setHours(0, 0, 0, 0);
              return selectedDate <= today || 'Date of birth cannot be in the future';
            }
          ]"
        />
        <q-select
          v-model="form.sex"
          :options="['Male', 'Female']"
          label="Sex"
          outlined
          dense
          hide-bottom-space
          :rules="[val => !!val || 'Sex is required']"
        />

        <q-separator class="q-mt-lg q-mb-sm" />

        <div class="q-mt-md">
          <div class="text-subtitle2 q-mb-sm">Custom Properties</div>
          <div v-for="(prop, index) in customProperties" :key="index" class="row q-gutter-sm items-center q-mb-sm">
              <q-input v-model="prop.name" label="Name" outlined dense class="col"
                hide-bottom-space
                :rules="[
                  val => !!val || 'Property name is required',
                  val => val.length <= 150 || 'Max 150 characters'
                ]"
              />
              <q-input v-model="prop.value" label="Value" outlined dense class="col"
                hide-bottom-space  
                :rules="[
                  val => !!val || 'Property value is required',
                  val => val.length <= 255 || 'Max 255 characters'
                ]"
              />
              
              <q-btn
                flat
                round
                dense
                icon="delete"
                color="negative"
                @click="customProperties.splice(index, 1)"
              />
            </div>

            <q-btn
              flat
              icon="add"
              label="Add Property"
              color="primary"
              @click="customProperties.push({ id: 0, name: '', value: '' })"
              class="q-mt-sm"
            />
          </div>
      </q-card-section>

      <q-card-actions align="right">
        <q-btn flat label="Cancel" v-close-popup />
        <q-btn color="primary" label="Save" @click="save" />
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>

<script setup lang="ts">
import { ref, watch, computed } from 'vue'
import { api } from 'boot/axios'
import type { CreateUpdateUserDto, UserDto } from 'src/types/user'

interface CustomProperty {
  id: number
  name: string
  value: string
}

const props = defineProps<{
  modelValue: boolean
  user: UserDto | null
}>()

const emit = defineEmits(['update:modelValue', 'saved'])

// Form state
const form = ref<CreateUpdateUserDto>({
  firstname: '',
  lastname: '',
  dateOfBirth: null,
  sex: '',
  customProperties: []
})

function resetForm() {
  form.value = {
    firstname: '',
    lastname: '',
    dateOfBirth: null,
    sex: '',
    customProperties: []
  };
  
  dateOfBirthString.value = '';
  customProperties.value = [];
}

// Custom properties array for UI
const customProperties = ref<CustomProperty[]>([])

// Date string binding for QInput type="date"
const dateOfBirthString = ref('')

// Edit mode
const isEdit = computed(() => !!props.user?.id)

// Sync prop into form
watch(
  () => props.user,
  (u) => {
    form.value = u
      ? {
          firstname: u.firstname,
          lastname: u.lastname,
          dateOfBirth: u.dateOfBirth ? new Date(u.dateOfBirth) : null,
          sex: u.sex,
          customProperties: u.customProperties || []
        }
      : { firstname: '', lastname: '', dateOfBirth: null, sex: '', customProperties: [] }

    dateOfBirthString.value = form.value.dateOfBirth
      ? form.value.dateOfBirth.toISOString().slice(0, 10)
      : ''

    customProperties.value = form.value.customProperties.map(p => ({ ...p }))
  },
  { immediate: true }
)

// Save function
async function save() {
  form.value.dateOfBirth = dateOfBirthString.value
    ? new Date(dateOfBirthString.value)
    : null

  form.value.customProperties = customProperties.value.map(p => ({
    id: p.id,
    name: p.name,
    value: p.value
  }))

  // Basic validation
  if (!form.value.firstname || !form.value.lastname || !form.value.dateOfBirth || !form.value.sex) {
    return
  }

  try {
    if (isEdit.value && props.user?.id) {
      await api.put(`/users/${props.user.id}`, form.value)
    } else {
      await api.post('/users', form.value)
    }

    emit('saved')
    resetForm();
    emit('update:modelValue', false)
  } catch (err) {
    console.error(err)
  }
}
</script>
