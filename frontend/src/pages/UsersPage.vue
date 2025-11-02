<template>
  <q-page padding>
    
    <div class="q-mx-auto" style="max-width: 1000px">
      <div class="row items-center justify-between q-mb-sm q-mt-md">
        <div class="text-h4">User Management</div>

        <div class="row q-gutter-sm">
          <q-btn label="Download Report" color="secondary" icon="download" @click="downloadReport()" />
          <q-btn label="Add User" color="primary" icon="add" @click="openModal()" />
        </div>
      </div>

      <q-table
        :rows="users"
        :columns="columns"
        row-key="id"
        @row-click="editUser"
      >
        <template v-slot:body-cell-dateOfBirth="props">
          <q-td class="text-right">
            {{ formatDate(props.row.dateOfBirth) }}
          </q-td>
        </template>

        <template v-slot:body-cell-actions="props">
          <q-td :props="props" class="q-gutter-sm">
            <q-btn
              icon="edit"
              color="secondary"
              flat
              round
              dense
              @click.stop="openModal(props.row)"
            >
              <q-tooltip>Edit User</q-tooltip>
            </q-btn>
            <q-btn
              icon="delete"
              color="negative"
              flat
              round
              dense
              @click.stop="confirmDelete(props.row)"
            >
              <q-tooltip>Delete User</q-tooltip>
            </q-btn>
          </q-td>
        </template>
      </q-table>

      <UserFormModal
        v-model="showModal"
        :user="selectedUser"
        @saved="fetchUsers"
      />
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { api } from 'boot/axios'
import type { UserDto } from 'src/types/user'
import UserFormModal from 'src/pages/UserFormModal.vue'
import { useQuasar } from 'quasar'
import { isAxiosError } from 'axios'

const $q = useQuasar()

const users = ref<UserDto[]>([])
const selectedUser = ref<UserDto | null>(null)
const showModal = ref(false)

// Table columns
const columns = [
  { name: 'firstname', label: 'First Name', field: 'firstname' },
  { name: 'lastname', label: 'Last Name', field: 'lastname' },
  { name: 'dateOfBirth', label: 'Date of Birth', field: 'dateOfBirth'},
  { name: 'sex', label: 'Sex', field: 'sex' },

  { name: 'actions', label: 'Actions', field: ''}
]

// Format date for display
function formatDate(date: Date | string) {
  const d = date instanceof Date ? date : new Date(date)
  return d.toISOString().slice(0, 10)
}

// Fetch users from backend
async function fetchUsers() {
  try {
    const res = await api.get<UserDto[]>('/users')
    users.value = res.data.map(u => ({ ...u, dateOfBirth: new Date(u.dateOfBirth) }))
  } catch (err) {
    console.error(err)
  }
}

// Open modal for adding user
function openModal(user: UserDto | null = null) {
  selectedUser.value = user
  showModal.value = true
}

async function downloadReport() {
  const endpoint = '/Users/export/xlsx';

  try {
    const response = await api.get(endpoint, {
      responseType: 'blob'
    });

    const blob = response.data;
    
    const contentDisposition = response.headers['content-disposition'];
    let filename = 'UserData.xlsx';

    if (contentDisposition) {
      const filenameMatch = contentDisposition.match(/filename\*?=['"]?([^;"]+)/i);
      if (filenameMatch && filenameMatch[1]) {
        filename = decodeURIComponent(filenameMatch[1].replace(/['"]/g, ''));
      }
    }

    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename; 

    document.body.appendChild(a);
    a.click();
    
    window.URL.revokeObjectURL(url);
    a.remove();

    console.log('Report downloaded successfully.');
    $q.notify({
      type: 'positive',
      message: 'Report downloaded successfully.'
    });

  } catch (error: unknown) {
    console.error('Error downloading report:', error);

    let status = 'N/A';
    let message = 'Failed to download report.';

    if (isAxiosError(error)) {
      if (error.response) {
        
        status = error.response.status.toString();
        message = `Failed to download report. Status: ${status}`;
      
      } else if (error.request) {
        status = 'Network Error';
        message = 'Failed to connect to the API server.';
      
      } else {
        message = `Request setup error: ${error.message}`;
      }
    } else if (error instanceof Error) {
      message = error.message;
    }

    $q.notify({
      type: 'negative',
      message: message
    });
  }
}

// Open modal for editing user
function editUser(_evt: Event, row: UserDto) {
  selectedUser.value = row
  showModal.value = true
}

function confirmDelete(user: UserDto) {
  $q.dialog({
    title: 'Confirm Delete',
    message: `Are you sure you want to delete ${user.firstname} ${user.lastname}? This action cannot be undone.`,
    color: 'negative',
    cancel: true,
    persistent: true
  }).onOk(() => {
    deleteUser(user.id).catch((err) => {
      console.error(err);
      $q.notify({
        type: 'negative',
        message: 'Failed to delete user.'
      });
    });
  });
}

async function deleteUser(id: number) {
  try {
    await api.delete(`/users/${id}`)
    $q.notify({
      type: 'positive',
      message: 'User deleted successfully.'
    })
    await fetchUsers()
  } catch (err) {
    console.error(err)
    $q.notify({
      type: 'negative',
      message: 'Failed to delete user.'
    })
  }
}

onMounted(fetchUsers)
</script>
